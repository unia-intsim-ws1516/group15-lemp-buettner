# Load the spectral power distributions

l = 300:1:790;

x = normpdf (l, 720, 25);
y = normpdf (l, 680, 25);
z = normpdf (l, 360, 75);

figure;
plot (l, x, l, y, l, z);

u = x / sqrt(trapz(l, x.*x));
v = y - trapz(l, y.*u)*u;
v = v / sqrt(trapz(v.*v));
w = z - trapz(l, z.*u)*u - trapz(l, z.*v)*v;
w = w / sqrt(trapz(l, w.*w));

figure;
plot (l, u, l, v, l, w);

img = imread("74.png");

[w, h, d] = size(img);

XYZ2RGB = [0.4124 0.3576 0.1805;
           0.2126 0.7152 0.0722;
           0.0193 0.1192 0.9505];

RGB2XYZ = [ 3.2406 -1.5372 -0.4986;
           -0.9689  1.8758  0.0415;
            0.0557 -0.2040  1.0570];

for j = 1:h
  for i = 1:w
    # convert rgb to xyz
    rgb = double(img(i,j,:) / 255);
    idx = (rgb <= 0.04045);
    xyz(idx)  = (rgb / 12.92)(idx);
    xyz(!idx) = (((rgb + 0.055)./(1.055)).^2.4)(!idx);
    
    xyz = XYZ2RGB * xyz(:);
    
    xyz(3) = 0;
    
    tmp = XYZ2RGB \ xyz;
    
    idx = tmp <= 0.0031308;
    rgb(idx) = (12.92*tmp)(idx);
    rgb(!idx) = (1.055*(tmp.^(1/2.4)) - 0.055)(!idx);
    
    urgb = uint8(rgb) * 255;
    urgb = min(urgb, ones(3)*255);
    urgb = max(urgb, zeros(3));
  endfor
endfor


imshow(img);