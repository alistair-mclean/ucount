from pprint import pprint
import cv2
import os

def make_dirs_for_channels_and_save_results(results):
	# This method creates both of the directories for the separate files 
	# and saves the results
	pa_summary = results[1]
	ecoli_summary = results[2]

	# Extract the original information
	original_image = results['original image']
	original_file_name = results['original file_name']
	base_file_name = original_file_name.split('.')[0]


	output_directory = os.path.join(os.getcwd(),results['output directory'])
	# Save the original
	write_name = os.path.join(output_directory, original_file_name)	
	print('The original file: ', write_name)

	# Try to make dir for Pseudonomas:
	print('[] \n [] \n output_directory: %s \n results[output_directory] = %s ' % (output_directory, results['output directory']))
	new_dir = output_directory + 'pseudonomas_preprocessed/'
	try:
		os.mkdir(new_dir)
	except Exception as e:
		print('88 88 88 Ran into an exception when trying to make directory')
		print('Exception: ', e)
		print('Directory: ', new_dir)
		pass	
	# print('Saving pseudonomas_preprocessed for ', original_file_name)

	# Save the threshold
	new_file_name = 'thresh_' + base_file_name + '.png'
	write_name = os.path.join(new_dir, new_file_name)  
	cv2.imwrite(r'{name}'.format(name=write_name), pa_summary['threshold channel'])

	# Save the preprocessed
	new_file_name = 'preproc_' + base_file_name + '.png'
	write_name = os.path.join(new_dir, new_file_name) 
	cv2.imwrite(r'{name}'.format(name=write_name), pa_summary['preprocessed channel'])


	# Try to make dir for E Coli:
	new_dir = output_directory + 'ecoli_preprocessed/'
	try:
		os.mkdir(new_dir)
	except FileExistsError:
		pass
	# print('Saving ecoli_preprocessed for ', original_file_name)

	# Save the threshold
	new_file_name = 'thresh_' + base_file_name + '.png'
	write_name = os.path.join(new_dir, new_file_name)  
	cv2.imwrite(r'{name}'.format(name=write_name), ecoli_summary['threshold channel'])

	# Save the preprocessed
	new_file_name = 'preproc_' + base_file_name + '.png'
	write_name = os.path.join(new_dir, new_file_name)  
	cv2.imwrite(r'{name}'.format(name=write_name), ecoli_summary['preprocessed channel'])
