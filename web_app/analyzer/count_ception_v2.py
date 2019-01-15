import sys, os, time, random
import tensorflow as tf
import numpy as np
matplotlib.use('Agg')
import matplotlib.pyplot as plt
plt.set_cmap('jet')

import skimage
from skimage.io import imread, imsave
import pickle
import scipy

from os import walk
import argparse


parser = argparse.ArgumentParser(description='Count-ception v2')

parser.add_argument('-seed', type=int, nargs='?',default=0, help='random seed for split and init')
parser.add_argument('-nsamples', type=int, nargs='?',default=32, help='Number of samples (N) in train and valid')
parser.add_argument('-stride', type=int, nargs='?',default=1, help='The args.stride at the initial layer')
parser.add_argument('-lr', type=float, nargs='?',default=0.00005, help='This will set the learning rate ')
parser.add_argument('-kern', type=str, nargs='?',default="sq", help='This can be gaus or sq')
parser.add_argument('-cov', type=float, nargs='?',default=1, help='This is the covariance when kern=gaus')
parser.add_argument('-scale', type=int, nargs='?',default=1, help='Scale the input image and labels')
parser.add_argument('-data', type=str, nargs='?',default="cells", help='Dataset folder')
parser.add_argument('-framesize', type=int, nargs='?',default=256, help='Size of the images processed at once')


job_id = os.environ.get('SLURM_JOB_ID')

if job_id == None:
	job_id = os.environ.get('PBS_JOBID')

print('job_id: %s' % job_id)