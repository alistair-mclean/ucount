from imageProcessor import *
from preprocessor import preprocess_all_images_in_dir
import numpy as np
import sklearn
import cv2
import sys
import os

class Analyzer():
	def __init__(self, image=None):
		self.original = image
		self.gray = None
		self.grays = []
		self.colors = []
		self.edges = []

	def analyze_images_in_dir(self, directory):
		print('\n---------- Analyzing images in directory: %s ----------\n' % directory)
		green_directory = os.path.join(directory, 'green_pp/')
		red_directory = os.path.join(directory, 'red_pp/')
		# TODO CHECK IF THE PRE PROCESSED DIRECTORIES EXIST

		print('----- Analyzing preprocessed green channels -----\n')
		files = [] 
		for subdir, dirs, file_names in os.walk(green_directory):
			files = file_names
		for file_name in files:
			image = cv2.imread(file_name)
			self.analyze_image(file_name, image)

		print('----- Analyzing preprocessed red channels -----\n')
		files = [] 
		for subdir, dirs, file_names in os.walk(red_directory):
			files = file_names
		for file_name in files:
			image = cv2.imread(file_name)
			self.analyze_image(file_name, image)

	def analyze_image(self, file_name, image):
		print('Analyzing %s' % file_name)
		# TODO - this is where the neural network kicks in!

if __name__=='__main__':
	analyzer = Analyzer()
	if len(sys.argv) < 2:
		print('[ERROR] Analyzer.__main__: Not enough arguments.')
		print('Please enter the directory you would to analyze.')
	else:
		directory = sys.argv[1]
		analyzer.analyze_images_in_dir(directory)
