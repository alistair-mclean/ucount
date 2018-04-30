# analyzer.py
# Author - Alistair McLean
# Purpose: Analyze various characteristics about organisms in an image
# TODO: 
#
#
#
import numpy as np
from pathlib import Path
import cv2
import datetime
import os
import ntpath
from src.analyzer.imageProcessor import PreProcessor
from src.analyzer.imageProcessor import PostProcessor
from src.test.tester import tester
class Organism():
	def __init__(self, id):
		self.id = id
	def __init__(self, name, colorRange, sizeRange, sensitivity, clustering=False):
		self.name = name
		self.colorRange = colorRange
		self.sizeRange = sizeRange
		self.sensitivity = sensitivity
		self.clustering = clustering

class Analyzer():
	def __init__(self, filename):
		self.filename = filename
		self.img = cv2.imread(self.filename)
		self.height, self.width, self.depth = self.img.shape
		self.envArea = self.height * self.width 
		self.organismList = []
		self.size = []
		self.organismCount = 0
		self.postProcessing = True

	def analyzeOrganism(self, organismName, sizeRange, colorRange): # NEW METHOD
		hsv = cv2.cvtColor(self.img, cv2.COLOR_BGR2HSV)
		self.size = sizeRange
		self.colorRange = [np.array(colorRange[0]), np.array(colorRange[1])]
		self.mask = cv2.inRange(hsv, self.colorRange[0], self.colorRange[1])
		self.result = cv2.bitwise_and(self.img, self.img, mask=self.mask)
		if self.postProcessing:
			self.postProcessed = PostProcessor().SharpenEdges(self.mask)

		self.countSpecies(self.mask)
		self.OutputResults(organismName) # debug	
		self.SaveResults(organismName)
		

	def identifyClusters(self, image, numberOfClusters, clusteringAlgo='kMeans'): # NEW METHOD
		# Put in a switch statement which determines which algorithm to use
		# Also put int the gottdang code!
		#  
		pass


	def countSpecies(self, mask): # NEW METHOD
		pxCount =  0
		for i in range(0, self.height):
			for j in range(0, self.width):
				if mask[i,j] > 0:
					pxCount = pxCount + 1

		self.coverage = 0.0
		self.pxCount = pxCount
		self.popRange = [int(pxCount / self.size[0]), int(pxCount / self.size[1])]
		if self.pxCount > 0:
			self.coverage =  self.pxCount/ self.envArea * 100.0

	def test(self): # NEW METHOD

		l_red = np.array([0, 97, 30]) # THESE STILL NEED MASSAGING
		u_red = np.array([10, 255, 255]) # THESE STILL NEED MASSAGING
		size = [5, 20]
		color = [l_red, u_red]
		organism = "herpaderp"
		self.analyzeOrganism(organism, size, color)
		cv2.imshow('mask', self.mask)
		cv2.imshow('result', self.result)
		cv2.waitKey(0)
	

	def OutputResults(self, organismName): # NEW METHOD
		# Print out the results to the console
		prettyDate = datetime.datetime.now().strftime("%Y-%m-%d at %H:%M:%S")
		print('-----------------------------------------------------------------------')
		print('                               BioAnalyzer                             ')
		print('-----------------------------------------------------------------------')
		print(' Source file: {}'.format(self.filename))
		print(' Organism: {}'.format(organismName))
		print(' Date: {}'.format(prettyDate))
		# TODO - PRINT THE THRESHOLDS
		print('=============================== RESULTS ===============================')
		print(' Area of image: {} pixels.'.format(self.envArea))
		print(' Population range of {} within {} and {} organisms.'.format(organismName, self.popRange[1], self.popRange[0]))
		print(' Size Range: [ {}px , {}px ]'.format(self.size[0], self.size[1]))
		print(' Based on the count of : {} pixels from the given thresholds: {} - {}.'.format(self.pxCount, self.colorRange[0], self.colorRange[1]))
		print(' Percentage of area covered by {} is: {}%'.format(organismName, self.coverage))
		print('-----------------------------------------------------------------------\n')
		cv2.waitKey(0)
		cv2.destroyAllWindows()

	def SaveResults(self, organismName): # NEW METHOD
		#Get the date for the new directory
		date = datetime.datetime.now().strftime("%Y%m%d_%H%M%S")
		prettyDate = datetime.datetime.now().strftime("%Y-%m-%d at %H:%M:%S")
		try:
			os.stat('results/' + date)
		except:
			os.mkdir('results/' + date)
		
		# Write to file
		filename = 'results/' + date + "/" + organismName + ".txt"
		f = open(filename, 'w')
		f.write('-----------------------------------------------------------------------\n')
		f.write(' Source file: {}\n'.format(self.filename))
		f.write(' Recorded on : {}\n'.format(prettyDate))
		f.write(' Organism: {}\n'.format(organismName))
		# TODO - PRINT THE THRESHOLDS
		f.write('=============================== RESULTS ===============================\n')
		f.write(' Area of image: {} pixels.\n'.format(self.envArea))
		f.write(' Population range of {} within {} and {} organisms.\n'.format(organismName, self.popRange[0], self.popRange[1]))
		f.write(' Size Range: [ {}px , {}px ]\n'.format(self.size[0], self.size[1]))
		f.write(' Based on the count of : {} pixels resulting from the given thresholds: {} - {}. \n'.format(self.pxCount, self.colorRange[0], self.colorRange[1]))
		f.write(' Percentage of area covered by {} is: {}%\n'.format(organismName, self.coverage))
		f.close()

		fname = ntpath.basename(self.filename)


		path = "results/" + date + "/images/"
		try:
			os.stat(path)
		except:
			os.mkdir(path)
		cv2.imwrite(path + "original.jpg", self.img)
		cv2.imwrite(path + organismName + "_Mask.jpg", self.mask)
		cv2.imwrite(path + organismName + "_Result.jpg", self.result)
		if self.postProcessing:
			cv2.imwrite(path + organismName + "_PostProcessed.jpg", self.postProcessed)

