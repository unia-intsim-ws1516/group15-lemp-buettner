#ifndef FUNCTION_UTILITY_H_
#define FUNCTION_UTILITY_H_

#include <vector>

typedef std::vector<double> Function;

/**
 * \param values Equidistant values of the Function to integrate. There has to
 * to be an odd number of values.
 * \param h Distance between two consecutive values in values.
 */
double integral(Function const& values, double const& h)
{
    double I = 0.0;
    for (int i = 0; i < static_cast<int>(values.size()) - 2; i += 2)
    {
        I += values[i] + 4.0 * values[i+1] + values[i+2];
    }

    return I * h / 3.0;
}


/**
 * Shifts inFunction with step size step by delta and stores the result in outFünction.
 */
void shiftFunction(Function const& inFunction, double step, double delta, Function& outFunction)
{
    int const di = static_cast<int>(std::floor(delta / step));
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

Function operator+(Function const& f, Function const& g)
{
    Function h(f.size());
    for (size_t i = 0; i < f.size(); ++i)
    {
        h[i] = f[i] + g[i];
    }
    
    return h;
}


Function operator-(Function const& f, Function const& g)
{
    Function h(f.size());
    for (size_t i = 0; i < f.size(); ++i)
    {
        h[i] = f[i] - g[i];
    }
    
    return h;
}

Function operator*(Function const& f, double const& c)
{
    Function g(f.size());
    for (size_t i = 0; i < f.size(); ++i)
    {
        g[i] = c * f[i];
    }

    return g;
}

Function operator*(double const& c, Function const& f)
{
    return f * c;
}

Function operator*(Function const& f, Function const& g)
{
    Function h(f.size());
    for (size_t i = 0; i < f.size(); ++i)
    {
        h[i] = f[i] * g[i];
    }
    return h;
}

Function operator/(Function const& f, double const& c)
{
    Function g(f.size());
    for (size_t i = 0; i < f.size(); ++i)
    {
        g[i] = f[i] / c;
    }
    return g;
}

#endif
