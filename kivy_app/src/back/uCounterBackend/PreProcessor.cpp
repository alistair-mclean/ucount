#include "PreProcessor.h"



PreProcessor::PreProcessor()
{
	
}

PreProcessor::PreProcessor(const cv::Mat & image, int clipLimit)
{
	_originalImage = image;
	// Separate Channels
	cv::Mat blur;
	cv::Size kSize = cv::Size(3, 3);
	cv::GaussianBlur(image, blur, kSize, 2, 2, cv::BORDER_DEFAULT);
	cv::imshow("blur", blur);
	cv::waitKey(0);
	std::vector<cv::Mat> colors = SeparateChannels(image);
	std::vector<cv::Mat> grays = GrayAndCLAHE(colors, clipLimit);
	std::vector<cv::Mat> colors2 = SeparateChannels(blur);
	std::vector<cv::Mat> grays2 = GrayAndCLAHE(colors2, clipLimit);
	EdgeDetection(grays[1]);
	EdgeDetection(grays2[1]);
	EdgeDetection(grays2[2]);
	
}

PreProcessor::PreProcessor(const cv::Mat & image, const settings& pSettings)
{
	_originalImage = image;
	cv::Mat blur;
	cv::Size kSize = cv::Size(pSettings.k_size, pSettings.k_size);
	cv::GaussianBlur(image, blur, kSize, pSettings.sigmaX, pSettings.sigmaY);
	std::vector<cv::Mat> colors = SeparateChannels(blur);
	std::vector<cv::Mat> grays = GrayAndCLAHE(colors, pSettings.clipLimit);

}


PreProcessor::~PreProcessor()
{
}

std::vector<cv::Mat> PreProcessor::SeparateChannels(const cv::Mat & image)
{
	std::vector<cv::Mat> ret;
	ret.push_back(image & cv::Scalar(255, 0, 0));
	ret.push_back(image & cv::Scalar(0, 255, 0));
	ret.push_back(image & cv::Scalar(0, 0, 255));
	return ret;
}

std::vector<cv::Mat> PreProcessor::GrayAndCLAHE(const std::vector<cv::Mat>& imageArray, int clipLimit)
{
	std::vector<cv::Mat> gray = imageArray;
	cv::cvtColor(imageArray[0], gray[0], cv::COLOR_BGR2GRAY); // Blue grayscale
	cv::cvtColor(imageArray[1], gray[1], cv::COLOR_BGR2GRAY); // Green grayscale
	cv::cvtColor(imageArray[2], gray[2], cv::COLOR_BGR2GRAY); // Red grayscale
	cv::imshow("PRECLAHE", gray[1]);

	cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE();
	clahe->setClipLimit(clipLimit);
	clahe->apply(gray[0], gray[0]);
	clahe->apply(gray[1], gray[1]);
	clahe->apply(gray[2], gray[2]);
	cv::imshow("POSTCLAHE", gray[1]);
	cv::waitKey(0);
	return gray;
}

std::vector<cv::Mat> PreProcessor::GrayAndCLAHE(const cv::Mat & image, int clipLimit)
{
	cv::Mat gray = image;
	cv::cvtColor(image, gray, cv::COLOR_BGR2GRAY); 
	cv::Ptr<cv::CLAHE> clahe = cv::createCLAHE();
	clahe->setClipLimit(clipLimit);
	clahe->apply(gray, gray);
	return gray;
}

void PreProcessor::EdgeDetection(const cv::Mat & image)
{
	// TODO: 
	// 1.) Understand what the parameters to the Laplacian do. 
	// 2.) Figure out the correct parameters to apply to our code
	int kernel_size = 9;
	int scale = 1;
	int delta = 0;
	int ddepth = CV_16S;
	cv::Mat lap = image;
	cv::Laplacian(image, lap, ddepth, kernel_size, scale, delta, cv::BORDER_DEFAULT);
	cv::imshow("laplacian1", lap);
	cv::waitKey(0);
}


int PreProcessor::CountObjects(const cv::Mat & image)
{
	return 0;
}
