"""Summary
"""
from src.analyzer.image_processing.imageProcessor import ImageProcessor
from src.analyzer.image_processing.preprocessor import preprocess_all_images_in_dir
from src.analyzer.image_processing.meta_data_extractor import read_metadata
from src.analyzer.utils import make_dirs_for_channels_and_save_results
from src.analyzer.math_utils import compute_coverage
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
	    metadata (dict): The metadata of the file
	    original_image (cv2.image): Original image object
	    output_dir (str): The output _RESULTS_ directory
	    output_path (str): The full name and path of the analyzed file to be saved 
	    settings (dict): The settings for the analyzer and img_processor
	"""
	
	def __init__(self, config):
		"""
		Initialize arguments
		
		Args:
		    config (dict): The config for the analyzer and img_processor
		"""
		self.metadata = {}
		self.config = config
		self.img_processor = ImageProcessor()

		self.output_dir = None
		self.output_path = None
		self.summary_dir = None

		self.original_image = None
		self.base_file_name = None

	def analyze_images_in_dir(self, dir_to_analyze):
		"""
		
		Analyzes all images in a directory ending with a .tif extension.
		
		If there is a config.json file in the directory (or subdirectory)
		the settings are loaded and used for the analysis.
		
		Generates a _RESULTS_ directory for each sub directory, where it stores
		the preprocessed images for E Coli and PA.

		Also has the summarizer write the summary to the original directory 
		being analyzed (NOT the _RESULTS_ directory). 
		
		Args:
		    dir_to_analyze (str): Description
		
		No Longer Returned:
		    - [String] Summaries array
		
		
		Args:
		    dir_to_analyze (TYPE): Description
		"""
		for subdir, dirs, file_names in os.walk(dir_to_analyze):
			files = [file_name for file_name in file_names if file_name.endswith('.tif')]
			
			# Iterate over all tif files in the directory that isn't the results dir.  
			if len(files) > 0 and '__RESULTS__' not in subdir:
				# If there is a settings file in the directory use that as the settings
				# for the Analyzer 
				if 'config.json' in file_names:
					with open(dir_to_analyze + 'config.json', 'r') as f:
						try:
							self.config = json.load(f)
						except Exception as e:
							print(e)

				self.output_dir = '%s__RESULTS__%s' % (dir_to_analyze, subdir[len(dir_to_analyze) - 1:])
				for file_name in files:
					try:
						os.mkdir(self.output_dir)
					except FileExistsError:
						pass
					self.base_file_name = file_name
					file_to_read = os.path.join(subdir, file_name)
					list_of_results = self.analyze_image(file_to_read)
					results = {
						'organisms': list_of_results,
						'original filename': self.base_file_name,
						'config': self.config,
						'output directory': self.output_dir
					}
					print('\t\t\t\tSAVING RESULTS')
					print('====================================================================')
					make_dirs_for_channels_and_save_results(results)
					print('Done.')

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

	def generate_results(self, img, organism):
		""" 
		Generates a result dict to then be saved in a _RESULTS_.txt file
		in the directory being analyzed. 
		
		Args:
		    channels (TYPE): Description
		"""
		# Takes some channels and applies a threshold based on 
		# the settings and returns a summary object
		threshold_settings = organism['config']['analysis']['threshold']
		threshold = self.img_processor.threshold_grayscale(img, threshold_settings)
		calculated_results = compute_coverage(threshold) 
		image_summary = {
			'name': organism['name'],
			'preprocessed image' : img,
			'threshold image' : threshold,
			'results' : calculated_results,
			'config': organism['config']
		}
		return image_summary


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
		list_of_results = []
		organism_results = {}
		if self.output_dir is None:
			# If an output directory has not been established, create one.
			self.base_file_name = file_name
			self.generate_base_dir_and_file_name_from_file_path()
		
		try:
			self.metadata = read_metadata(file_name)
			self.original_image = cv2.imread(file_name)
		except Exception as e:
			print('[ERROR] Analyzer had an issue reading: ', file_name)
			print(e)
		print('\t\t\t\tANALYZING')
		print('====================================================================')
		for organism in self.config['organisms']:
			print('--------------------------------------------------------------------')
			print('\tAnalyzing for %s in %s. ' % (organism['name'],file_name))

			filter_mode = organism['config']['filter mode']
			self.img_processor.update_config(organism['config']['preprocessing'])
			self.img_processor.update_mode(filter_mode)
			image = self.original_image			
			if filter_mode == 'HSV':
				image = cv2.cvtColor(self.original_image, cv2.COLOR_BGR2HSV)
			channels = self.img_processor.process_image(image)
			results = self.generate_results(channels, organism)
			list_of_results.append(results)
			print('--------------------------------------------------------------------')

		print('====================================================================')		
		return list_of_results

	def analyze_image_single(self, filename):
		list_of_results = self.analyze_image(filename)
		self.output_dir = '%s__RESULTS__' % self.output_dir
		try:
			os.mkdir(self.output_dir)
		except FileExistsError:
			pass
		results = {
			'organisms': list_of_results,
			'original filename': self.base_file_name,
			'config': self.config,
			'output directory': self.output_dir
		}
		print('\t\t\t\tSAVING RESULTS')
		print('====================================================================')
		make_dirs_for_channels_and_save_results(results)
		print('Done.')


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
