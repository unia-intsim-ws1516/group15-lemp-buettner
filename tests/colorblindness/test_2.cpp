#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <tclap/CmdLine.h>
#include <iostream>
#include <limits>
#include <Eigen/Eigen>
#include <cie_rgb_standard_observers.h>
#include <cie_lms_sensitivity.h>
#include <functionutility.h>

using namespace cv;
using namespace std;
using namespace TCLAP;
using namespace Eigen;

int main (int argc, char* argv[])
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
        int const n = 1000;
        Function x(n);
        Function y(n);
        Function z(n);
    
        for (int i = 300; i < x.size(); ++i)
        {
            x[i] = 1.0/(2.0*M_PI) * exp(-(i - 350)*(i - 350)/(2.0*50*50));
            y[i] = 1.0/(2.0*M_PI) * exp(-(i - 550)*(i - 550)/(2.0*25*25));
            z[i] = 1.0/(2.0*M_PI) * exp(-(i - 650)*(i - 650)/(2.0*25*25));
        }
    
        double const h = 1.0;
    
        double const norm_x = sqrt(integral(x*x, h));
        Function u = x / norm_x;
        Function v = y - integral(u*y, h)*u;
        v = v / sqrt(integral(v*v, h));
        Function w = z - integral(u*z, h)*u - integral(v*z, h)*v;
        w = w / sqrt(integral(w*w, h));
    
        Matrix3d XYZ2RGB;
        XYZ2RGB << 0.4124, 0.3576, 0.1805,
                   0.2126, 0.7152, 0.0722,
                   0.0193, 0.1192, 0.9505;
    
        Matrix3d LMS2XYZ;
        LMS2XYZ << 0.7328, 0.4296, -0.1624,
                  -0.7036, 1.6975,  0.0061,
                   0.0030, 0.0136,  0.9834;
        FullPivLU<Matrix3d> XYZ2LMS(LMS2XYZ);
    
        Matrix3d RGB2XYZ;
        RGB2XYZ <<  3.2406, -1.5372, -0.4986,
                   -0.9689,  1.8758,  0.0415,
                    0.0557, -0.2040,  1.0570;
    
        Matrix3d CVD = Matrix3f::Identity();
        for (int y = 0; y < image.rows; ++y)
        {
            for (int x = 0; x < image.cols; ++x)
            {
                Vec3b px = image.at<Vec3b>(y, x);
                Vector3d p(px.val[2], px.val[1], px.val[0]);
                
                // rgb to xyz
                Vector3d c1 = p / 12.92;
                Vector3d c2 = ((p + 0.055 * Vector3d::Ones()) / 1.055).array().pow(2.4);
                Vector3d q = (p.array() <= 0.04045).select(c1, c2);
    
                Vector3d xyz = XYZ2RGB * q;
                Vector3d lms = LMS2XYZ * xyz;
    
                // Project
//                lms.y() = lms.x();
                lms.z() = 0.0;
    
                xyz = XYZ2LMS.solve(lms);
                // xyz to rgb
                q = RGB2XYZ * xyz;
                c1 = 12.92 * q;
                c2 = 1.055 * q.array().pow(1.0/2.4) - 0.055;
                p = (q.array() <= 0.031308).select(c1, c2);
    
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
    } // disableColorProcessing

    imwrite("uiae.png", image);
    imshow("Some caption", image);
    waitKey(0);
    return 0;
}

