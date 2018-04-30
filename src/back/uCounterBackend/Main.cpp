#include "ImageProcessor.h"
#include "Statistics.h"


int main(int argc, char** argv) {
	cv::Mat image = cv::imread("../../../samples/Mix_Well1_3Teflon_new.tif");
	Statistics stats;
	stats.CreateHistogramHSV(image, 256, true, false);
	ImageProcessor::settings settings = ImageProcessor::settings();
	settings.k_size = 3;
	settings.scale = 1;
	settings.delta = 0;
	settings.sigmaX = 2;
	settings.sigmaY = 2;
	settings.ddepth = CV_16S;
	ImageProcessor prep = ImageProcessor(image, settings);
}