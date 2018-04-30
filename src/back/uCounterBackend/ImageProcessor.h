#pragma once
#include "opencv2\opencv.hpp"
#include <vector>


class ImageProcessor
{
public:
	struct settings {
		int k_size;
		int scale;
		int delta;
		int sigmaX;
		int sigmaY;
		int clipLimit;
		int ddepth;
		settings() {
			int k_size = 3;
			int scale = 1;
			int delta = 0;
			int sigmaX = 0;
			int sigmaY = 0;
			int clipLimit = 2;
			int ddepth = CV_16S;
		}

		settings(int kSize, int scl, int dlta, int sigX, int sigY, int clpLim, int ddpth) {
			int k_size = kSize;
			int scale = scl;
			int delta = dlta;
			int sigmaX = sigX;
			int sigmaY = sigY;
			int clipLimit = clpLim;
			int ddepth = ddpth;
		}
	};
	ImageProcessor();
	ImageProcessor(const cv::Mat& image, int clipLimit);
	ImageProcessor(const cv::Mat& image, const settings& pSettings);
	~ImageProcessor();

	std::vector<cv::Mat> SeparateChannels(const cv::Mat& image);
	std::vector<cv::Mat> GrayAndCLAHE(const std::vector<cv::Mat>& imageArray, int clipLimit);
	std::vector<cv::Mat> GrayAndCLAHE(const cv::Mat& image, int clipLimit);
	void EdgeDetection(const cv::Mat& image);
	int CountObjects(const cv::Mat& image);

private:
	cv::Mat _originalImage;
};

