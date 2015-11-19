#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <tclap/CmdLine.h>
#include <iostream>
#include <limits>
#include <Eigen/Eigen>
#include <cie_rgb_standard_observers.h>
#include <cie_lms_sensitivity.h>

using namespace cv;
using namespace std;
using namespace TCLAP;
using namespace Eigen;


/**
 * \param values Equidistant values of the function to integrate. There has to
 * to be an odd number of values.
 * \param h Distance between two consecutive values in values.
 */
double integral(vector<double> const& values, double const& h)
{
    double I = 0.0;
    for (int i = 0; i < static_cast<int>(values.size()) - 2; i += 2)
    {
        I += values[i] + 4.0 * values[i+1] + values[i+2];
    }

    return I * h / 3.0;
}

void shiftFunction(vector<double> const& inFunction, double step, double delta, vector<double>& outFunction)
{
    int const di = static_cast<int>(floor(delta / step));
    int const dj = di + 1;
    double const t = (delta / step) - static_cast<double>(di);
    outFunction.resize(inFunction.size());

//    cout << "di = " << di << ", dj = " << dj << ", t = " << t << endl;
    for (size_t i = 0; i < inFunction.size(); ++i)
    {
        int const m = i + di;
        int const n = i + dj;
//        cout << "m = " << m << ", n = " << n << endl;
        double y1 = 0.0;
        double y2 = 0.0;
        if ( (0 <= m) && (m < inFunction.size()) ) y1 = inFunction[m];
        if ( (0 <= n) && (n < inFunction.size()) ) y2 = inFunction[n];
        outFunction[i] = (1.0 - t) * y1 + t * y2;
//        cout << "y1 = " << y1 << ", y2 = " << y2 << ", f = " << outFunction[i] << endl;
    }
}


class LambdaMatrix
{
public:
    LambdaMatrix(vector<double> const& r, vector<double> const& g, vector<double> const& b)
    : r_(r)
    , g_(g)
    , b_(b)
    {}

    Matrix3d operator() (vector<double> const& L, vector<double> const& M, vector<double> const& S, double step) const
    {
        vector<double> WS(L.size());
        vector<double> YB(L.size());
        vector<double> RG(L.size());
    
        Matrix3d const oppTlms( (Matrix3d() << 0.600,  0.400,  0.000,
                                               0.240,  0.105, -0.700,
                                               1.200, -1.600,  0.400).finished());
    
        for (size_t i = 0; i < L.size(); i++)
        {
            Vector3d const lms(L[i], M[i], S[i]);
            Vector3d const wyr = oppTlms * lms;
            WS[i] = wyr[0];
            YB[i] = wyr[1];
            RG[i] = wyr[2];
        }
    
        Matrix3d lambda;
    
        vector<double> mul(WS.size());
     
        // WS_R
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = r_[i] * WS[i];
        lambda(0,0) = integral(mul, step); // 5nm steps
     
        // WS_G
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = g_[i] * WS[i];
        lambda(0,1) = integral(mul, step); // 5nm steps
     
        // WS_B
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = b_[i] * WS[i];
        lambda(0,2) = integral(mul, step); // 5nm steps
     
        // YB_R
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = r_[i] * YB[i];
        lambda(1,0) = integral(mul, step); // 5nm steps
     
        // YB_G
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = g_[i] * YB[i];
        lambda(1,1) = integral(mul, step); // 5nm steps
     
        // YB_B
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = b_[i] * YB[i];
        lambda(1,2) = integral(mul, step); // 5nm steps
     
        // RG_R
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = r_[i] * RG[i];
        lambda(2,0) = integral(mul, step); // 5nm steps
     
        // RG_G
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = g_[i] * RG[i];
        lambda(2,1) = integral(mul, step); // 5nm steps
     
        // RG_B
        for (size_t i = 0; i < mul.size(); ++i)
            mul[i] = b_[i] * RG[i];
        lambda(2,2) = integral(mul, step); // 5nm steps
    
    
        lambda.row(0) /= lambda(0,0) + lambda(0,1) + lambda(0,2);
        lambda.row(1) /= lambda(1,0) + lambda(1,1) + lambda(1,2);
        lambda.row(2) /= lambda(2,0) + lambda(2,1) + lambda(2,2);
    
        return lambda;
    }

private:
    vector<double> const r_;
    vector<double> const g_;
    vector<double> const b_;
};


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

    vector<double> R(cie_r.size());
    vector<double> G(cie_g.size());
    vector<double> B(cie_b.size());

    double max_r = -numeric_limits<double>::max();
    double max_g = -numeric_limits<double>::max();
    double max_b = -numeric_limits<double>::max();
    for (size_t i = 0; i < R.size(); ++i)
    {
        max_r = max(max_r, cie_r[i]);
        max_g = max(max_g, cie_g[i]);
        max_b = max(max_b, cie_b[i]);
    }
    for (size_t i = 0; i < R.size(); ++i)
    {
        R[i] = cie_r[i] / max_r;
        G[i] = cie_g[i] / max_g;
        B[i] = cie_b[i] / max_b;
    }

//    vector<double> cie_l_norm(cie_l.size());
//    vector<double> cie_m_norm(cie_m.size());
//    vector<double> cie_s_norm(cie_s.size());
//    double max_l = -numeric_limits<double>::max();
//    double max_m = -numeric_limits<double>::max();
//    double max_s = -numeric_limits<double>::max();
//    for (size_t i = 0; i < cie_l.size(); ++i)
//    {
//        max_l = max(max_l, cie_l[i]);
//        max_m = max(max_m, cie_m[i]);
//        max_s = max(max_s, cie_s[i]);
//    }
//    double const max_lms = max(max_l, max(max_m, max_s));
//    for (size_t i = 0; i < cie_l.size(); ++i)
//    {
//        cie_l_norm[i] = cie_l[i] / max_lms;
//        cie_m_norm[i] = cie_m[i] / max_lms;
//        cie_s_norm[i] = cie_s[i] / max_lms;
//    }
    double const area_L = integral(cie_l, 5.0);
    double const area_M = integral(cie_m, 5.0);
    double const area_S = integral(cie_s, 5.0);

    vector<double> L;
    vector<double> M;
    vector<double> S;
    double const shift = 2.0;
//    shiftFunction(cie_l, 5.0,-shift, L);
//    shiftFunction(cie_m, 5.0,  0.0, M);
//    shiftFunction(cie_s, 5.0,  0.0, S);

    double const alpha = (20.0 - shift) / 20.0;
    for (size_t i = 0; i < L.size(); ++i)
    {
        L[i] = alpha * cie_l[i] + (1.0 - alpha) * area_L / area_M * 0.96 * cie_m[i];
    }

    LambdaMatrix lmb(R, G, B);
//    LambdaMatrix lmb(cie_r, cie_g, cie_b);
    Matrix3d const lambda_normal = lmb(cie_l, cie_m, cie_s, 5.0);
    FullPivLU<Matrix3d> const lambda_normal_inv(lambda_normal);

    Matrix3d const lambda = lmb(L, M, S, 5.0);
    cout << "lambda:" << endl << lambda << endl << endl;

    Matrix3f CVD(Matrix3f::Identity());
    CVD = lambda_normal_inv.solve(lambda);
    cout << "CVD:" << endl << CVD << endl << endl;

    for (int y = 0; y < image.rows; ++y)
    {
        for (int x = 0; x < image.cols; ++x)
        {
            Vec3b px = image.at<Vec3b>(y, x);
            Vector3f p(px.val[2], px.val[1], px.val[0]);
            p = CVD*p;
            if (123 == x && 321 == y)
            {
                cout << "px[" << (int)px.val[2] << ", " << (int)px.val[1] << ", " << (int)px.val[0] << "] ";
                cout << "p[" << p.transpose() << "] ";
                cout << endl;
            }
            px.val[0] = min(255.0f, max(0.0f, p[2]));
            px.val[1] = min(255.0f, max(0.0f, p[1]));
            px.val[2] = min(255.0f, max(0.0f, p[0]));
            image.at<Vec3b>(y, x) = px;
        }
    }

    imwrite("uiae.png", image);
    imshow("Some caption", image);
    waitKey(0);

    return 0;
}
