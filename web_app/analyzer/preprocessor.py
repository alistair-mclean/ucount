from imageProcessor import ImageProcessor
import sys
import cv2
import os

def preprocess_all_images_in_dir(directory):
	try:
		print('First try with the path: ', directory)
		for subdir, dirs, files in os.walk(directory):
			for filename in files: 
				print(filename)
				base_name, extension = filename.split('.')
				if not base_name.endswith('scale') and 'tif' in extension:
					processor = ImageProcessor()
					channels = []	
					try: 
						image = cv2.imread(filename)
						channels = processor.process_image(image)
						make_dirs_for_channels_and_save(directory, channels, base_name)
					except Exception as e:
						print('[WARNING] Preprocessor: Exception was thrown: ', e)
						try:
							filename_and_path = os.path.join(subdir, filename)
							image = cv2.imread(filename_and_path)
							channels = processor.process_image(filename_and_path)
							print(len(channels))
							make_dirs_for_channels_and_save(directory, channels, base_name)
						except Exception as e:
							print('[ERROR] Preprocessor: Failed to preprocess file. Exiting...')
	except Exception as e:
		print(e)
		try: 
			path = os.getcwd() + '/' + directory
			print('Second try with the path: ', path)
			for subdir, dirs, files in os.walk(path):
				for filename in files: 
					base_name, extension = filename.split('.')
					if not base_name.endswith('scale') and 'tiff' in extension:
						filename_and_path = os.path.join(subdir, filename)
						# Do the preprocessing
		except Exception as e:
			print('[ERROR] Preprocessor: The path provided does not exist. Exiting')
			return


def make_dirs_for_channels_and_save(directory, channels, base_file_name):
	# Try to make dir for green:
	try:
		print('Saving channel[0] for ', base_file_name)
		new_dir = directory + '/green_pp/'
		os.mkdir(new_dir)
		cv2.imwrite(base_file_name + '.png', channels[0])
	except Exception as e:
		print(e)	

	# Try to make dir for red:
	try:
		print('Saving channel[1] for ', base_file_name)
		new_dir = directory + '/green_pp/'
		os.mkdir(new_dir)
		cv2.imwrite(base_file_name + '.png', channels[1])
	except Exception as e:
		print(e)	






if __name__ == '__main__':
	directory = sys.argv[1]
	print('The provided directory was: ', directory)
	preprocess_all_images_in_dir(directory)