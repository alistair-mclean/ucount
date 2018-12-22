from imageProcessor import *
import numpy as np
import sklearn
import cv2

class Analyzer():
	def __init__(self, image=None):
		self.original = image
		self.gray = None
		self.grays = []
		self.colors = []
		self.edges = []

	def test(self, image):
		self.original = image
		proc = ImageProcessor()
		self.colors = proc.split_channels(image)
		idx = 0
		for img in self.colors:
			self.colors[idx] = cv2.bitwise_and(self.original, self.original, mask = img)
			idx += 1

		for img in self.colors:
			cv2.imshow('color', img)
			cv2.waitKey(0)
			cv2.destroyAllWindows()

	def analyzeImage(self, image):
		self.original = image
		proc = ImageProcessor()
		self.results = proc.processImage(self.original)
		idx = 0
		for resultantImage in self.results: 
			if idx >= 1:
				self.detectCircles(resultantImage)
			idx += 1

	def detectCircles(self, img, houghSettings=None):
		#convert to compatible datatype
		try:
			if houghSettings is None:
				circles = cv2.HoughCircles(img, cv2.HOUGH_GRADIENT, 8, 10, param1=50,param2=60,minRadius=5,maxRadius=10)
			else:
				circles = cv2.HoughCircles(img, cv2.HOUGH_GRADIENT, 8, 10, 
										   houghSettings.param1,houghSettings.param2,
										   houghSettings.minRadius,houghSettings.maxRadius)	
			circles = np.uint16(np.around(circles))
			for i in circles[0,:]:
				cv2.circle(img,(i[0],i[1]),i[2],(255,255,0),1)
			cv2.imshow('detected circles', img)
			cv2.waitKey(0)
			cv2.destroyAllWindows()
		except:
			print('Couldnt detect circles in the image.')
			return

if __name__=='__main__':
	analyzer = Analyzer()
	path = '~/research/uCounter/web_app/samples/'
	filename = 'Mix_Well1_2Steel_new.tif'
	file = path + filename
	image = cv2.imread(filename)
	cv2.imshow('Original', image)
	cv2.waitKey(0)
	cv2.destroyAllWindows()
	analyzer.analyzeImage(image)


