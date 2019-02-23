from PIL import Image
from PIL.TiffTags import TAGS
import sys, os
from pprint import pprint

def read_metadata(file_name):
	meta_dict = {}
	with Image.open(file_name) as img:
		meta_dict = {TAGS[key] : img.tag[key] for key in img.tag}
	return meta_dict


if __name__ == '__main__':
	if len(sys.argv) >= 2:
		file_name = sys.argv[1]
		full_path = os.path.join(os.getcwd(), file_name)
		meta_data = read_metadata(full_path)
		intro_message ='\n============== Extracted meta_data for %s ==============' % file_name 
		length = len(intro_message)
		print(intro_message)
		pprint(meta_data)
		end_line = ''
		for i in range(length - 1):
			end_line += '='
		end_line += '\n'
		print(end_line)
