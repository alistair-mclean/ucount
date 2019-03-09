from src.analyzer.image_processing.imageProcessor import ImageProcessor
import sys
import cv2
import os

def preprocess_all_images_in_dir(directory):
	print('\n-------------- Preprocessing files in directory: %s --------------' % directory)
	try:
		print('First try with the path: ', directory)
		for subdir, dirs, files in os.walk(directory):
			print('In subdir: %s \n With Dirs: %s ' % (subdir, dirs))
			for filename in files: 
				print(filename)
				base_name, extension = filename.split('.')

				if base_name.endswith('scale'):
					pass

				elif 'tif' in extension:
					processor = ImageProcessor()
					channels = []	

					try: 
						full_name_and_path = os.path.join(directory, filename)
						print('FULL NAME AND PATH OF FILE: %s' % full_name_and_path)
						image = cv2.imread(full_name_and_path)

						# Process the image
						channels = processor.process_image(image)
						print("\n \n @@ @@@ channels.lenght = ", len(channels))
						if len(channels) > 0:
							make_dirs_for_channels_and_save(directory, channels, base_name)
						else:
							print('Skipping ')

					except Exception as e:
						print('[WARNING] Preprocessor: Exception was thrown: ', e)
						print(type(e))

						try:
							filename_and_path = os.path.join(subdir, filename)
							image = cv2.imread(filename_and_path)
							channels = processor.process_image(filename_and_path)
							print(len(channels))
							make_dirs_for_channels_and_save(directory, channels, base_name)
						except Exception as e:
							print(type(e))
							print('[ERROR] Preprocessor: Failed to preprocess file. Exiting...')
	except Exception as e:
		print(type(e))
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
			print(type(e))
			print('[ERROR] Preprocessor: The path provided does not exist. Exiting')
			return


def make_dirs_for_channels_and_save(directory, channels, base_file_name):
	# Try to make dir for Pseudonomas:
	new_dir = directory + '/pseudonomas_preprocessed/'
	try:
		os.mkdir(new_dir)
	except FileExistsError:
		pass	
	print('Saving pseudonomas_preprocessed for ', base_file_name)
	write_name = new_dir + base_file_name + '.png' 
	cv2.imwrite(r'{name}'.format(name=write_name), channels[0])


	# Try to make dir for E Coli:
	new_dir = directory + '/ecoli_preprocessed/'
	try:
		os.mkdir(new_dir)
	except FileExistsError:
		pass
	print('Saving ecoli_preprocessed for ', base_file_name)
	write_name = new_dir + base_file_name + '.png' 
	cv2.imwrite(r'{name}'.format(name=write_name), channels[1])


if __name__ == '__main__':
	directory = sys.argv[1]
	print('The provided directory was: ', directory)
	preprocess_all_images_in_dir(directory)