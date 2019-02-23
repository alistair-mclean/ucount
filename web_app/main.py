from server.analyzer.analyzer import Analyzer
import argparse
import sys
import os

def parse_arguments():
	parser = argparse.ArgumentParser()
	parser.add_argument("square", help="display a square of a given number",
                    type=int)
	args = parser.parse_args()
	print(args.square**2)



if __name__ == '__main__':
	parse_arguments()