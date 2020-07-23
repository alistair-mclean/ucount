from pprint import pprint
import csv
import cv2
import sys
import json
import os

IMAGE_EXTENSION = '.png'
SUMMARY_EXTENSION = '.csv'
CONFIG_EXTENSION = '.json'
ALLOWED_EXTENSIONS = set(['jpg', 'png', 'tif'])

def make_dirs_for_channels_and_save_results(results):
	# This method creates both of the directories for the separate files 
	# and saves the results
	# Extract the original information
	original_file_name = results['original filename']
	base_file_name = original_file_name.split('.')[0]

	output_directory = os.path.join(os.getcwd(),results['output directory'])
	write_name = os.path.join(output_directory, original_file_name)	

	organisms = results['organisms']

	for organism in organisms:
		
		new_dir = os.path.join(output_directory, organism['name'].replace(" ", "_"))
		try:
			os.mkdir(new_dir)
		except FileExistsError:
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
	organism_results = {}

	for organism in results['organisms']:
		organism_results.update({organism['name'] : organism['results']})
	original_file_name = results['original filename']
	base_file_name = original_file_name.split('.')[0]
	output_directory = os.path.join(os.getcwd(),results['output directory'])

	summary_file_name = 'SUMMARY_' + base_file_name + SUMMARY_EXTENSION
	config_file_name = 'CONFIG_' + base_file_name + CONFIG_EXTENSION

	output_summary_path = os.path.join(output_directory, summary_file_name)
	output_config_path = os.path.join(output_directory, config_file_name)
	
	# Record the config
	with open(output_config_path, 'w') as jsonFile:
		jsonFile.write(json.dumps(results['config']))

	with open(output_summary_path, 'w') as csvFile:
		csv_writer = None
		headers = ['Name', 'Percent coverage', 'Approximate coverage', 'Coverage units', 'Image resolution']
		csv_writer = csv.DictWriter(csvFile, fieldnames=headers)
		csv_writer.writeheader()

		for organism in organism_results:
			result_keys = [result for result in organism_results[organism]]
			full_keys = ['Name']
			full_keys.extend(result_keys)

			csv_writer.writerow({
				'Name': organism,
				'Percent coverage': organism_results[organism]['Percent coverage'],
				'Approximate coverage': organism_results[organism]['Approximate coverage'],
				'Coverage units': organism_results[organism]['Coverage units'],
				'Image resolution': organism_results[organism]['Image resolution'],
				
			})
		
