from server.analyzer.imageProcessor import *
from server.analyzer.preprocessor import preprocess_all_images_in_dir
from server.analyzer.meta_data_extractor import read_metadata
import numpy as np
import sklearn
import cv2
import sys
import os

class Analyzer():
	def __init__(self):
		self.gray = None
		self.grays = []
		self.colors = []
		self.edges = []

	def analyze_images_in_dir(self, directory):
		for subdir, dirs, file_names in os.walk(directory):
			files = []
			files = [file_name for file_name in file_names if file_name.endswith('.tif')]
			for file_name in files:
				self.analyze_image(file_name)
			if len(files) > 0:
				output_dir = '%sRESULTS%s' % (directory, subdir[len(directory) -1:])
				print('OUTPUT DIR: %s ' % output_dir)


	def write_results(self, results):
		pass



	def analyze_image(self, file_name):
		print('Analyzing %s' % file_name)
		# TODO - this is where the neural network kicks in!


	def extract_metadata(self, file_name):
		metadata = read_metadata(file_name)


if __name__=='__main__':
	analyzer = Analyzer()
	if len(sys.argv) < 2:
		print('[ERROR] Analyzer.__main__: Not enough arguments.')
		print('Please enter the directory you would to analyze.')
	else:
		directory = sys.argv[1]
		analyzer.analyze_images_in_dir(directory)
