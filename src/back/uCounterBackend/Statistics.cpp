#include "Statistics.h"



Statistics::Statistics()
{
}

void Statistics::CreateHistogram(const cv::Mat & image, int numBins, bool uniform, bool accumulate)
{
	float range[] = { 0, 256 };
	const float* histRange = { range };
	cv::Mat hist;
}

void Statistics::CreateHistogramHSV(const cv::Mat & image, int numBins, bool uniform, bool accumulate)
{
	float range[] = { 0, 256 };
	const float* histRange = { range };
	cv::Mat hsv;
	cv::cvtColor(image, hsv, cv::COLOR_BGR2HSV);
	// Quantize the hue to 30 levels
	// and the saturation to 32 levels
	int hbins = 30, sbins = 32;

	int histSize[] = { hbins, sbins };
	// Hue varies from 0 to 179
	float hranges[] = { 0, 180 };
	// Saturation varies from 0 to 255
	float sranges[] = { 0, 256 };
	const float* ranges[] = { hranges, sranges };

	cv::MatND hist;
	// Compute the histogram from the 0-th and 1-st channels 
	int channels[] = { 0, 1 };

	cv::calcHist(&hsv, 1, channels, cv::Mat(), hist, 2, histSize, ranges, uniform, accumulate);
	double maxVal = 0;
	cv::minMaxLoc(hist, 0, &maxVal, 0, 0);

	int scale = 10;
	cv::Mat histImg = cv::Mat::zeros(sbins*scale, hbins*scale, CV_8UC3);
	for (int h = 0; h < hbins; h++) {
		for (int s = 0; s < sbins; s++) {
			float binVal = hist.at<float>(h, s);
			int intensity = cvRound(binVal * 255 / maxVal);
			cv::rectangle(histImg, cv::Point(h*scale, s*scale), cv::Point((h + 1)*scale - 1, (s + 1)*scale - 1),
				cv::Scalar::all(intensity), CV_FILLED);
		}
	}
	cv::namedWindow("Source", 1);
	cv::imshow("Source", image);
	cv::namedWindow("H-S Histogram", 1);
	cv::imshow("H-S Histogram", histImg);
	cv::waitKey(0);
}


Statistics::~Statistics()
{
}
