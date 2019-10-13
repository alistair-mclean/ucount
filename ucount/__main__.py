from .src.analyzer.analyzer import Analyzer
import argparse
import json
import sys
import os

def parse_arguments():
	parser = argparse.ArgumentParser()
	parser.add_argument("--filename", help="The filename to analyze.")
	parser.add_argument("--directory", help="The directory to analyze.")
	parser.add_argument("--config", help="A config file for custom execution.")
	args = parser.parse_args()
	config = {
		"preprocessing" : {
			"clahe" : {
				"clip_limit": 1.5,
				"k_size": 10
			},
			"blur" : {
				"k_size": 3
			}
		},
		"analysis" : {
			"threshold" : {
				"max": 255,
				"min": 35
			}
		}
	}

	# If config were supplied, try to assign the settings.
	if args.config:
		try:
			json_data = open(args.config).read()
			config = json.loads(json_data)
		except Exception as e:
			print('[WARNING] Improper settings file, using defaults.')
			print(e)

	# Initialize the Analyzer
	analyzer = Analyzer(config)

	if args.filename:
		# If the user entered a filename then analyze the file
		try:
			analyzer.analyze_image_single(args.filename)
		except Exception as e:
			print(e)
	else:
		# Otherwise try to analyze the directory if one was provided
		try:
			if args.directory:
				try:
					analyzer.analyze_images_in_dir(args.directory)
					# analyzer.test_preprocessor(args.directory)
				except Exception as e:
					print(e)
		except Exception as e:
			print (e)



if __name__ == '__main__':
	parse_arguments()
