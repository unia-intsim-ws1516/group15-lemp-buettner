#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <tclap/CmdLine.h>
#include <iostream>
#include <Eigen/Eigen>

using namespace cv;
using namespace std;
using namespace TCLAP;
using namespace Eigen;

int main(int argc, char* argv[])
{
    CmdLine cmdParser("Test program", ' ', "strange version");

    ValueArg<string> fileName("f", "file", "Image file", true, "", "path");
    cmdParser.add(fileName);
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

    Matrix3f CVD;
    CVD << 0.259411, 0.923008, -0.182420,
           0.110296, 0.804340, 0.085364,
           -0.006276, -0.034346, 1.040622;
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
    namedWindow("Some Window", WINDOW_AUTOSIZE);
    imshow("Some caption", image);
    waitKey(0);

    return 0;
}
