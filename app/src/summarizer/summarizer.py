from pprint import pprint 
import json
import sys
import csv

class Summarizer():	
	def __init__(self, directory=None):
		self.directory = directory # The directory that the results are being saved in. 
		self.summaries = []
		self.resulting_summary = [] # The array of lines to write in the output file.

	def summarize_results_in_dir(self, directory=None, summaries=[]):
		self.summaries = summaries
		self.directory = directory

		self.add_to_result('================================= RESULTS FOR : %s ================================= ' % self.directory)
		self.add_to_result('FILENAME : %s' % self.filename)
		for summary in self.summaries:
			self.compose_summary(summary)

	def compose_summary(self,summary):
		filename = summary['filename']
		organism1 = summary['organism1']
		organism2 = summary['organism2']
		settings = summary['settings']
		self.add_to_result('%s PERCENT AREA COVERED: %f ' %(organism1['name'], organism1['coverage']))
		self.add_to_result('%s PERCENT AREA COVERED: %f ' %(organism2['name'], organism2['coverage']))
		self.compose_settings(settings)

	def compose_settings(self, settings):
		self.add_to_result('SETTINGS: ')
		self.add_to_result('\tIMAGE AREA: %s' % settings['analysis']['image_area'])
		self.add_to_result('\tAREA UNITS: %s' % settings['analysis']['area_units'])
		self.add_to_result('\tPREPROCESSING:')
		self.add_to_result('\t\tCLAHE:')
		self.add_to_result('\t\t\tCLIP LIMIT: %s' % settings['preprocessing']['clahe']['clip_limit'])
		self.add_to_result('\t\t\tKERNEL SIZE: %s' % settings['preprocessing']['clahe']['k_size'])
		self.add_to_result('\t\tGAUSSIAN BLUR:')
		self.add_to_result('\t\t\tKERNEL SIZE: %s' % settings['preprocessing']['blur']['k_size'])
		self.add_to_result('\tANALYSIS:')
		self.add_to_result('\t\tTHRESHOLD VALUES')
		self.add_to_result('\t\t\tMAX: %s' % settings['analysis']['threshold']['max'])
		self.add_to_result('\t\t\tMIN: %s' % settings['analysis']['threshold']['min'])
		self.add_to_result('\n')

		
	def print_resulting_summary(self):
		for line in self.resulting_summary:
			print(line)

	def add_to_result(self, str):
		self.resulting_summary.append(str)


# if __name__ == '__main__':
# 	testfile = 'example_summary_datastructure.json'
# 	summary = {}
# 	jsummary = None
# 	summarizer = Summarizer()
# 	with open(testfile, 'r') as f:
# 		summary = json.loads(f.read())
# 	pprint(summary)
# 	pprint(summary['settings'])
# 	summarizer.compose_summary(summary)
# 	summarizer.print_resulting_summary()