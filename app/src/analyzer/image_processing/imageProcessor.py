import cv2
import numpy as np
import matplotlib.pyplot as plt

class ImageProcessor(): 
	def __init__(self, mode=None, settings=None, original=None):
		self.mode = mode
		self.original = []
		self.settings = {}
		if settings:
			self.settings = settings
		if original:
			self.original = cv2.imread(original)
			return self.process_image(self.original)

	def process_image(self, img):
		self.original = img

		contrast_settings = {
			'clip_limit': 1.5,
			'k_size': 10
		}
		blur_settings = {
			'k_size' : 3
		}
		if self.settings:
			contrast_settings = self.settings['clahe']
		if self.settings:
			blur_settings = self.settings['blur']


		try:
			channels = self.split_channels(img)
		except Exception as e:
			print('[ERROR] ImageProcessor.process_image: failed to split_channels on img: ', img)
		indx = 0
		for channel in channels[1:]:
			channel = self.smooth(channel, blur_settings['k_size']) # k_size = 3 seems the most ideal..
			# cv2.imshow('smooth', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()
			
			channel = self.improve_contrast(channel, contrast_settings) 
			# cv2.imshow('clahe', channel)
			# cv2.waitKey(0)
			# cv2.destroyAllWindows()

			channels[indx] = channel
			indx += 1
		return channels

	def test_show_img(self, img):
		cv2.imshow('image', img)
		cv2.waitKey(0)
		cv2.destroyAllWindows()


	# Split the channels of the image into
	def split_channels(self, img):
		channels = []
		try:
			channels = cv2.split(img)
		except Exception as e:
			print('[ERROR] ImageProcessor.split_channels: Exception occurred: ', e)
			raise Exception
		return channels

	# Apply a laplacian convolution 
	def highlight_edges(self, img):		
		try:
			edges = cv2.Laplacian(img, cv2.CV_8UC1)
		except:
			print("[ERROR] ImageProcessor.highlight_edges: Could't detect the edges!")
			return None
		return edges

	def improve_contrast(self, img, contrast_settings=None):
		clip_limit = 3.0
		k_size = (5,5)
		if contrast_settings:
			clip_limit = contrast_settings['clip_limit']
			k_size = (contrast_settings['k_size'], contrast_settings['k_size'])
		try:
			clahe = cv2.createCLAHE(clipLimit=clip_limit, tileGridSize=k_size)
			temp = clahe.apply(img)
		except:
			print("[ERROR] ImageProcessor.improve_contrast: Couldn't improve the contrast.")
			return None
		return temp
		
	def threshold_image(self, image, threshold_settings):
		if threshold_settings['min'] > 255:
			threshold_settings['min'] = 255
		if threshold_settings['max'] > 255:
			threshold_settings['max'] = 255
		if threshold_settings['min'] < 0:
			threshold_settings['min'] = 0
		if threshold_settings['max'] < 0:
			threshold_settings['max'] = 0

		ret, thet = cv2.threshold(image, threshold_settings['min'], threshold_settings['max'], cv2.THRESH_OTSU)
		return thet
		

	def normalize_image(self, img):
		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX)
		return img

	# Apply a gaussian blur
	def smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))



