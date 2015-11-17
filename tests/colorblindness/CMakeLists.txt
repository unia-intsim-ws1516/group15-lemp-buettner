cmake_minimum_required(VERSION 2.8 FATAL_ERROR)

find_package(OpenCV REQUIRED)

include_directories(/usr/include/eigen3)
add_executable(colorblindness main.cpp)
target_link_libraries(colorblindness ${OpenCV_LIBS})

