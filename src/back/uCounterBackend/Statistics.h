#pragma once
#include "opencv2\opencv.hpp"
class Statistics
{
public:
	Statistics();
	void CreateHistogram(const cv::Mat& image, int numBins, bool uniform, bool accumulate);
	void CreateHistogramHSV(const cv::Mat& image, int numBins, bool uniform, bool accumulate);
	void CreateHistogram(const cv::Mat& image, int numBins, bool accumulate);
	~Statistics();
};

