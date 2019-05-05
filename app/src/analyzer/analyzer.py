"""Summary
"""
from src.analyzer.image_processing.imageProcessor import ImageProcessor
from src.analyzer.image_processing.preprocessor import preprocess_all_images_in_dir
from src.analyzer.image_processing.meta_data_extractor import read_metadata
from src.analyzer.utils import make_dirs_for_channels_and_save_results
import numpy as np
import json
import sklearn
import cv2
import sys
import os
from pprint import pprint

class Analyzer(object):

	"""
	
	Attributes:
	    base_file_name (str): Original file name currently being analyzed
	    img_processor (ImageProcessor): The ImageProcessor object
	    metadata (dict): The metadata of the file
	    original_image (cv2.image): Original image object
	    output_dir (str): The output _RESULTS_ directory
	    output_path (str): The full name and path of the analyzed file to be saved 
	    settings (dict): The settings for the analyzer and img_processor
	"""
	
	def __init__(self, settings):
		"""
		Initialize arguments
		
		Args:
		    settings (dict): The settings for the analyzer and img_processor
		"""
		self.metadata = {}
		self.settings = settings
		self.img_processor = ImageProcessor(self.settings['preprocessing'])

		self.output_dir = None
		self.output_path = None
		self.summary_dir = None

		self.original_image = None
		self.base_file_name = None

	def analyze_images_in_dir(self, directory):
		"""
		
		Analyzes all images in a directory ending with a .tif extension.
		
		If there is a settings.json file in the directory (or subdirectory) 
		the settings are loaded and used for the analysis.
		
		Generates a _RESULTS_ directory for each sub directory, where it stores
		the preprocessed images for E Coli and PA.

		Also has the summarizer write the summary to the original directory 
		being analyzed (NOT the _RESULTS_ directory). 
		
		Args:
		    directory (str): Description
		
		No Longer Returned:
		    - [String] Summaries array
		
		
		Args:
		    directory (TYPE): Description
		"""
		for subdir, dirs, file_names in os.walk(directory):
			files = []
			files = [file_name for file_name in file_names if file_name.endswith('.tif')]

			summaries = []
			# Iterate over all tif files in the directory that isn't the results dir.  
			if len(files) > 0 and '__RESULTS__' not in subdir:
				# If there is a settings file in the directory use that as the settings
				# for the Analyzer 
				if 'settings.json' in file_names:
					with open(directory + 'settings.json', 'rb') as f:
						self.settings = json.load(f)
						
				self.output_dir = '%s__RESULTS__%s/' % (directory, subdir[len(directory) -1:])
				for file_name in files:
					try:
						os.mkdir(self.output_dir)
					except FileExistsError:
						pass
					self.base_file_name = file_name
					path = os.path.join(directory, subdir[len(directory) -1:])
					file_to_read = os.path.join(subdir, file_name)
					summary = self.analyze_image(file_to_read)
					summaries.append(summary)
					self.output_path = os.path.join(self.output_dir, file_name)
				

	def test_preprocessor(self, directory):
		""" 
		Test function for preprocessor
		
		Args:
		    directory (str): The directory to preprocess
		"""
		preprocess_all_images_in_dir(directory)

	def test_show_img(self, img, title='image'):
		""" 
		Test function to display an image
		
		Args:
		    img (cv2.image): The image to display
		    title (str, optional): The title of the image window.
		"""
		cv2.imshow(title, img)
		cv2.waitKey(0)
		cv2.destroyAllWindows()

	def generate_results(self, channels):
		""" 
		Generates a result dict to then be saved in a _RESULTS_.txt file
		in the directory being analyzed. 
		
		Args:
		    channels (TYPE): Description
		"""
		# Takes some channels and applies a threshold based on 
		# the settings and returns a summary object
		settings = self.settings['analysis']['threshold']
		thresholds = [self.img_processor.threshold_image(channel, settings) for channel in channels]
		calculated_results = [self.calculate_results(threshold) for threshold in thresholds]
		summary = {
				'settings' : settings,
  			    'original image' : self.original_image,
  			    'original file_name' : self.base_file_name,
  			    'output directory' : self.output_dir,
		}
		index = 0
		for threshold in thresholds:
			name = 'E Coli'
			if index > 1:
				name = 'Pseudonomas'
			channel_summary = {
				'name': name,
				'preprocessed channel' : channels[index],
				'threshold channel' : thresholds[index],
				'results' : calculated_results[index]
			}
			summary.update({index : channel_summary})
			index += 1
		return summary


	def calculate_results(self, image):
		"""
		
		Args:
		    image (TYPE): Description
		
		Returns:
		    TYPE: Description
		"""
		height, width = np.shape(image)
		area = height * width
		sum_of_pixels = 0 
		for row in image:
			for px in row:
				if px > 245:
					sum_of_pixels += 1
		percent_covered = float(sum_of_pixels) / float(area) * 100.0
		results = {
			'Percent coverage' : float('%.2f' % percent_covered),
			'Image resolution' : [height, width],
		}
		return results

	def generate_base_dir_and_file_name_from_file_path(self):
		"""
		Generates the output_dir for the analyzer from the base_file_name
		"""
		path_without_extension = self.base_file_name.split('.')
		split_up_children = []
		if '/' in path_without_extension[0]:
			split_up_children = path_without_extension[0].split('/')
			self.base_file_name = self.base_file_name.split('/')[-1]
		elif '/' not in path_without_extension[0]:
			split_up_children = path_without_extension[0].split('\\')
			self.base_file_name = self.base_file_name.split('\\')[-1]

		pprint(path_without_extension)
		pprint(split_up_children)
		print(path_without_extension[:len(path_without_extension) - len(split_up_children[-1]) - 1])	
		base_directory = path_without_extension[0][:len(path_without_extension[0]) - len(split_up_children[-1])]
		self.base_file_name = self.base_file_name.split('/')[-1]
		self.output_dir = base_directory

	def analyze_image(self, file_name):
		"""
		Analyzes an image, but first preprocesses
		
		Args:
		    file_name (TYPE): Description
		"""
		results = {}
		if self.output_dir is None:
			self.base_file_name = file_name
			self.generate_base_dir_and_file_name_from_file_path()
		
		try:
			self.metadata = read_metadata(file_name)
			self.original_image = cv2.imread(file_name)

			print('---------------------------------------------------------------------')
			print('Analyzing %s with settings: ' % file_name)
			pprint(self.settings)
			self.img_processor.settings = self.settings['preprocessing']
			channels = self.img_processor.process_image(self.original_image)
			results = self.generate_results(channels)
			print('---------------------------------------------------------------------')

			print('Results: ')
			# pprint(results)
			# print(results[0]['name'])
			# print(results[0]['results'])
			print('Name: ' + results[1]['name'])
			pprint(results[1]['results'])
			print('Name: ' + results[2]['name'])
			pprint(results[2]['results'])
			print('====================================================================')
			make_dirs_for_channels_and_save_results(results)
		
		except Exception as e:
			print('[ERROR] Analyzer had an issue reading: ', file_name)
			print(e)

	def extract_metadata(self, file_name):
		"""Summary
		
		Args:
		    file_name (TYPE): Description
		"""
		metadata = read_metadata(file_name)


if __name__=='__main__':
	analyzer = Analyzer()
	if len(sys.argv) < 2:
		print('[ERROR] Analyzer.__main__: Not enough arguments.')
		print('Please enter the directory you would to analyze.')
	else:
		directory = sys.argv[1]
		analyzer.analyze_images_in_dir(directory)
