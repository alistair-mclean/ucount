from pprint import pprint
import csv
import cv2
import json
import os

IMAGE_EXTENSION = '.png'
SUMMARY_EXTENSION = '.csv'

def make_dirs_for_channels_and_save_results(results):
	# This method creates both of the directories for the separate files 
	# and saves the results
	# Extract the original information
	original_image = results['original image']
	original_file_name = results['original file_name']
	base_file_name = original_file_name.split('.')[0]


	output_directory = os.path.join(os.getcwd(),results['output directory'])
	# Save the original
	write_name = os.path.join(output_directory, original_file_name)	
	for organism in results['organisms']:
		# pprint(organism)
		# Try to make dir for Pseudonomas:
		new_dir = output_directory + '_' + organism['name'].replace(" ", "_")

		try:
			os.mkdir(new_dir)
		except FileExistsError	:
			pass	

		# Save the threshold
		new_file_name =  base_file_name + '_threshold' + IMAGE_EXTENSION
		write_name = os.path.join(new_dir, new_file_name)  
		cv2.imwrite(r'{name}'.format(name=write_name), organism['threshold image'])

		# Save the preprocessed
		new_file_name = base_file_name + '_preprocessed' + IMAGE_EXTENSION
		write_name = os.path.join(new_dir, new_file_name)
		cv2.imwrite(r'{name}'.format(name=write_name), organism['preprocessed image'])
		
	write_summary(results)

def write_summary(results):
	# print(results)
	original_file_name = results['original file_name']
	base_file_name = original_file_name.split('.')[0]
	output_directory = os.path.join(os.getcwd(),results['output directory'])
	summary_file_name = 'SUMMARY_' + base_file_name + SUMMARY_EXTENSION
	output_summary_path = os.path.join(output_directory, summary_file_name)
	config_file_name = 'CONFIG_' + base_file_name + '.json'
	output_config_path = os.path.join(output_directory, config_file_name)
	
	# Record the config
	with open(output_config_path, 'w') as jsonFile:
		jsonFile.write(json.dumps(results['config']))

	with open(output_summary_path, 'w') as csvFile:
		csv_writer = csv.writer(csvFile, quoting=csv.QUOTE_MINIMAL)
		for organism in results['organisms']:
			results = organism['results']
			csv_writer.writerow([result for result in results])
		
