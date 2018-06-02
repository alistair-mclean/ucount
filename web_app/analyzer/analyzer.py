from imageProcessor import *
import numpy as np
import sklearn
import cv2

# TODO: 
# + Massage the values for CLAHE so that we get a better result 
#	from  the laplacian


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
		self.colors = proc.splitChannels(image)
		idx = 0
		for img in self.colors:
#			cv2.imshow('color', img)
#			cv2.waitKey(0)
#			cv2.destroyAllWindows()
			self.colors[idx] = cv2.bitwise_and(self.original, self.original, mask = img)
			idx += 1

		for img in self.colors:
			cv2.imshow('color', img)
			cv2.waitKey(0)
			cv2.destroyAllWindows()

	def analyzeImage(self, image):
		self.original = image
		proc = ImageProcessor()
		self.colors = proc.splitChannels(image)
		self.grays.append(proc.preProcess(self.colors[0]))
		self.grays.append(proc.preProcess(self.colors[1]))
		self.grays.append(proc.preProcess(self.colors[2]))
#		self.edges = proc.sharpenEdges(self.grays[:])
		print('Displaying grays')
		cv2.imshow('b', self.grays[0])
		cv2.imshow('g', self.grays[1])
		cv2.imshow('r', self.grays[2])
		cv2.waitKey(0)
		cv2.destroyAllWindows()

		print('Displaying edges')
		self.edges.append(proc.sharpenEdges(self.grays[1]))
		self.edges.append(proc.sharpenEdges(self.grays[2]))
		cv2.imshow('g', self.edges[0])
		cv2.imshow('r', self.edges[1])
		cv2.waitKey(0)
		cv2.destroyAllWindows()

		self.countCircles(self.edges[0])
		
	def countCircles(self, img):
		#circles = cv2.HoughCircles(img,cv2.HOUGH_GRADIENT,1,20,
        #                          param1=50,param2=30,minRadius=0,maxRadius=0)

		circles = cv2.HoughCircles(img,cv2.HOUGH_GRADIENT,1,20,
                            param1=50,param2=30,minRadius=0,maxRadius=0)
		circles = np.uint16(np.around(circles))
		for i in circles[0,:]:
			# Draw the outer circle
			cv2.circle(img, (i[0],i[1]), 2, (255,255,0),2)
		cv2.imshow('detected circles', img)
		cv2.waitKey(0)
		cv2.destroyAllWindows()

if __name__=='__main__':
	analyzer = Analyzer()
	path = '~/research/uCounter/web_app/samples/'
	filename = 'Mix_Well1_2Steel_new.tif'
	file = path + filename
	print(file) # Debug
	image = cv2.imread(filename)
	print(image.shape) # Debug
	#analyzer.analyzeImage(image)
	analyzer.test(image)


