cmake_minimum_required(VERSION 2.8 FATAL_ERROR)

find_package(OpenCV REQUIRED)

set (CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11")

include_directories(/usr/include/eigen3)
include_directories(./)
add_executable(colorblindness main.cpp)
target_link_libraries(colorblindness ${OpenCV_LIBS})

add_executable(colorblindness2 test_2.cpp)
target_link_libraries(colorblindness2 ${OpenCV_LIBS})

