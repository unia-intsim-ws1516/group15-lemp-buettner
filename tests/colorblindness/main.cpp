#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <tclap/CmdLine.h>
#include <iostream>
#include <limits>
#include <Eigen/Eigen>
#include <cie_rgb_standard_observers.h>
#include <cie_lms_sensitivity.h>
#include <cie_xyz_standard_observers.h>
#include <functionutility.h>

using namespace cv;
using namespace std;
using namespace TCLAP;
using namespace Eigen;

// Vector3d xyzTrgb(Vector3d const&);
// Vector3d rgbTxyz(Vector3d const&);


/* Trafo from sRGB to XYZ */
static Matrix3d const XYZ2RGB((Matrix3d() << 0.4124, 0.3576, 0.1805,
                                             0.2126, 0.7152, 0.0722,
                                             0.0193, 0.1192, 0.9505).finished());

static Matrix3d const RGB2XYZ((Matrix3d() << 3.2406, -1.5372, -0.4986,
                                            -0.9689,  1.8758,  0.0415,
                                             0.0557, -0.2040,  1.0570).finished());

int main(int argc, char* argv[])
{
    CmdLine cmdParser("Test program", ' ', "strange version");

    ValueArg<string> fileName("f", "file", "Image file", true, "", "path");
    SwitchArg disableColorProcessing("", "disable", "Disable color processing", false);
    cmdParser.add(fileName);
    cmdParser.add(disableColorProcessing);
    cmdParser.parse(argc, argv);

    Mat image;
    image = imread(fileName.getValue(), CV_LOAD_IMAGE_COLOR);

    if ( !image.data )
    {
        cerr << "Error reading image '" << fileName.getValue() << "'." << endl;
        return -1;
    }
    else
    {
        cout << "Loaded '" << fileName.getValue() << "'." << endl;
    }
    
    if (!disableColorProcessing.getValue())
    {

        double const h = 5.0; // nm
    
        vector<double> L(cie_l.size());
        vector<double> M(cie_m.size());
        vector<double> S(cie_s.size());
    
        double max_l = -numeric_limits<double>::max();
        double max_m = -numeric_limits<double>::max();
        double max_s = -numeric_limits<double>::max();
        for (size_t i = 0; i < L.size(); ++i)
        {
            max_l = max(max_l, cie_l[i]);
            max_m = max(max_m, cie_m[i]);
            max_s = max(max_s, cie_s[i]);
        }
        for (size_t i = 0; i < L.size(); ++i)
        {
            L[i] = cie_l[i] / max_l;
            M[i] = cie_m[i] / max_m;
            S[i] = cie_s[i] / max_s;
        }
        
        /*
         * Compute Lampda_normal
         */
        Matrix3d LNormal;
        LNormal(0,0) = integral(L * cie_x, h);
        LNormal(0,1) = integral(L * cie_y, h);
        LNormal(0,2) = integral(L * cie_z, h);
        LNormal(1,0) = integral(M * cie_x, h);
        LNormal(1,1) = integral(M * cie_y, h);
        LNormal(1,2) = integral(M * cie_z, h);
        LNormal(2,0) = integral(S * cie_x, h);
        LNormal(2,1) = integral(S * cie_y, h);
        LNormal(2,2) = integral(S * cie_z, h);
    
        LNormal = LNormal * XYZ2RGB;
    //    LNormal.row(0) = LNormal.row(0) / (LNormal(0,0) + LNormal(0,1) + LNormal(0,2));
    //    LNormal.row(1) = LNormal.row(1) / (LNormal(1,0) + LNormal(1,1) + LNormal(1,2));
    //    LNormal.row(2) = LNormal.row(2) / (LNormal(2,0) + LNormal(2,1) + LNormal(2,2));
    
        FullPivLU<Matrix3d> const LNormalInv(LNormal);
    
        /*
         * Compute Lambda with the altered LMS functions.
         */
        vector<double> L2(L);
        vector<double> M2(M);
        vector<double> S2(S);
    
        // shift by 15 nm as suggested in Machado
//        shiftFunction(L, h,  15.0, L2);
//        shiftFunction(M, h, -19.0, M2);
        L2 = 0.0 * L;
    
        Matrix3d Lambda = LNormal;
        Lambda(0,0) = integral(L2 * cie_x, h);
        Lambda(0,1) = integral(L2 * cie_y, h);
        Lambda(0,2) = integral(L2 * cie_z, h);
        Lambda(1,0) = integral(M2 * cie_x, h);
        Lambda(1,1) = integral(M2 * cie_y, h);
        Lambda(1,2) = integral(M2 * cie_z, h);
        Lambda(2,0) = integral(S2 * cie_x, h);
        Lambda(2,1) = integral(S2 * cie_y, h);
        Lambda(2,2) = integral(S2 * cie_z, h);
    
        Lambda = Lambda * XYZ2RGB;
    
    //    Lambda.row(0) = Lambda.row(0) / (Lambda(0,0) + Lambda(0,1) + Lambda(0,2));
    //    Lambda.row(1) = Lambda.row(1) / (Lambda(1,0) + Lambda(1,1) + Lambda(1,2));
    //    Lambda.row(2) = Lambda.row(2) / (Lambda(2,0) + Lambda(2,1) + Lambda(2,2));
    
        Matrix3d CVD = Matrix3d::Identity();
        CVD = LNormalInv.solve(Lambda);
    
        CVD.row(0) = CVD.row(0) / (CVD(0,0) + CVD(0,1) + CVD(0,2));
        CVD.row(1) = CVD.row(1) / (CVD(1,0) + CVD(1,1) + CVD(1,2));
        CVD.row(2) = CVD.row(2) / (CVD(2,0) + CVD(2,1) + CVD(2,2));
    
        cout << "CVD: " << endl << CVD << endl;
    
        for (int y = 0; y < image.rows; ++y)
        {
            for (int x = 0; x < image.cols; ++x)
            {
                Vec3b px = image.at<Vec3b>(y, x);
                Vector3d p(px.val[2], px.val[1], px.val[0]);
    
                p = CVD * p;
                if (123 == x && 321 == y)
                {
                    cout << "px[" << (int)px.val[2] << ", " << (int)px.val[1] << ", " << (int)px.val[0] << "] ";
                    cout << "p[" << p.transpose() << "] ";
                    cout << endl;
                }
                px.val[0] = min(255.0, max(0.0, p[2]));
                px.val[1] = min(255.0, max(0.0, p[1]));
                px.val[2] = min(255.0, max(0.0, p[0]));
                image.at<Vec3b>(y, x) = px;
            }
        }
    }

    imwrite("uiae.png", image);
    imshow("Some caption", image);
    waitKey(0);

    return 0;
}


// Vector3d xyzTrgb(Vector3d const& rgb)
// {
// 
//     // rgb to xyz
//     Vector3d const c1 = rgb / 12.92;
//     Vector3d const c2 = ((rgb + 0.055 * Vector3d::Ones()) / 1.055).array().pow(2.4);
//     Vector3d const q = (rgb.array() <= 0.04045).select(c1, c2);
// 
//     return XYZ2RGB * q;
// }
// 
// Vector3d rgbTxyz(Vector3d const& xyz)
// {
//     Vector3d const q = RGB2XYZ * xyz;
//     Vector3d const c1 = 12.92 * q;
//     Vector3d const c2 = 1.055 * q.array().pow(1.0/2.4) - 0.055;
//     return (q.array() <= 0.031308).select(c1, c2);
// }

