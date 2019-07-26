import cv2
from pprint import pprint
import numpy as np
import matplotlib.pyplot as plt

class ImageProcessor(): 
	def __init__(self, mode=None, settings=None):
		self.mode = mode
		self.original = []
		self.config = {}
		if settings:
			self.config = settings

	def process_image(self, img):
		self.original = img
		return self.process_image_using_mode(img)

	def process_image_using_mode(self, img):
		# Really missing switch statements right about now..
		if self.mode is None:
			raise Exception # TODO - Specify this exception 
		if self.mode == 'BGR':
			return self.process_image_via_bgr(img)
		elif self.mode == 'HSV':
			return self.process_image_via_hsv(img)
		elif self.mode == 'GRY':
			return self.process_image_via_grayscale(img)
		else:
			print("[WARNING] ImageProcessor.process_image_using_mode: Couldn't determine which settings to use. Defaulting to grayscale.")
			return self.process_image_via_grayscale(img)

	def process_image_via_bgr(self, img):
		# TODO - make settings a requirement.
		## TODO - refactor the image processor to instead use the new filter mode logic
		## 		  and the filter values for segmentation of images.
		bgr = cv2.cvtColor(img, cv2.COLOR_HSV2BGR)
		try:
			channels = self.split_channels_bgr(bgr)
			preprocessed = [self.preprocess_image(channel) for channel in channels]
			index = self.get_bgr_index()
			return preprocessed[index]
		except Exception as e:
			print('[ERROR] ImageProcessor.process_image_via_bgr: failed to split_channels_bgr on img: ', img)
			print('Exception: ', e)
			raise Exception # TODO - Specify this exception

	def process_image_via_hsv(self, img):
		try:
			segmented = self.segment_via_hsv(img)
			return self.preprocess_image(segmented)
		except Exception as e:
			print('[ERROR] ImageProcessor.process_image_via_hsv: Encountered exception: ', e)
			raise Exception # TODO - Specify this exception
		
	def process_image_via_grayscale(self, img):
		try:
			return self.preprocess_image(img)
		except Exception as e:
			print('[ERROR] ImageProcessor.process_image_via_grayscale: Encountered exception: ', e)
			raise Exception # TODO - Specify this exception

	def preprocess_image(self, channel):
		if self.config:
			contrast_settings = self.config['clahe']
		if self.config:
			blur_settings = self.config['blur']
		try:
			if blur_settings['on']:
				channel = self.smooth(channel, blur_settings['k_size']) # k_size = 3 seems the most ideal..
		except Exception as e:
			print('[ERROR] ImageProcessor.preprocess_image encountered exception when smoothing:', e)
			raise Exception # TODO - Specify this exception
		# cv2.imshow('smooth', channel)
		# cv2.waitKey(0)
		# cv2.destroyAllWindows()
		try:
			if contrast_settings['on']:	
				channel = self.improve_contrast(channel, contrast_settings) 
		except Exception as e:
			print('[ERROR] ImageProcessor.preprocess_image encountered exception when claheing:', e)
			raise Exception # TODO - Specify this exception
		# cv2.imshow('clahe', channel)
		# cv2.waitKey(0)
		# cv2.destroyAllWindows()
		return channel

	# Split the channels of the image into separate images 
	def split_channels_bgr(self, img):
		channels = []
		try:
			channels = cv2.split(img)
		except Exception as e:
			print('[ERROR] ImageProcessor.split_channels_bgr: Exception occurred: ', e)
			raise Exception # TODO - Specify this exception
		return channels

	# Segments the image via hsv
	def segment_via_hsv(self, img):
		try:
			min = np.array(self.config['filter values']['min'])
			max = np.array(self.config['filter values']['max'])
			mask = cv2.inRange(img, min, max)
			result = cv2.bitwise_and(img, img, mask=mask)
			color = cv2.cvtColor(result, cv2.COLOR_HSV2BGR)
			gray = cv2.cvtColor(color, cv2.COLOR_BGR2GRAY)
		except Exception as e:
			print('[ERROR] ImageProcessor.segment_via_hsv: Exception ocurrred: ', e)
			raise Exception # TODO - Specify this exception
		return gray

	# Apply a laplacian convolution 
	def highlight_edges(self, img):		
		try:
			edges = cv2.Laplacian(img, cv2.CV_8UC1)
		except:
			print("[ERROR] ImageProcessor.highlight_edges: Could't detect the edges!")
			return None
		return edges

	# Apply a gaussian blur
	def smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))

	def improve_contrast(self, img, contrast_settings=None):
		clip_limit = 3.0
		k_size = (5,5)
		if contrast_settings:
			clip_limit = contrast_settings['clip_limit']
			k_size = (contrast_settings['k_size'], contrast_settings['k_size'])
		try:
			clahe = cv2.createCLAHE(clipLimit=clip_limit, tileGridSize=k_size)
			temp = clahe.apply(img)
		except Exception as e:
			print(e)
			print(type(e))
			print("[ERROR] ImageProcessor.improve_contrast: Couldn't improve the contrast.")
			return None
		return temp
			
	def threshold_hsv(self, hsv_img, threshold_settings):
		if hsv_img is None:
			print('[ERROR] ImageProcessor.threshold_hsv: None type image was provided.')
			raise Exception # TODO - Specify this exception
		try:
			
			mask = cv2.inRange(hsv_img, lower, upper)
			result = cv2.bitwise_and(hsv_img, hsv_img, mask=mask)
			color = cv2.cvtColor(result, cv2.COLOR_HSV2BGR)
			gray = cv2.cvtColor(color, cv2.COLOR_BGR2GRAY)
			return gray
		except Exception as e:
			print(type(e)) 
			print("[ERROR] ImageProcessor.threshold_hsv: Ran into exception:", e)

	def threshold_grayscale(self, channel, threshold_settings):
		if channel is None:
			print('[ERROR] ImageProcessor.threshold_grayscale: None type channel supplied as argument.')
			raise Exception # TODO - Specify this exception
		try:
			ret, thet = cv2.threshold(channel, threshold_settings['min'], threshold_settings['max'], cv2.THRESH_OTSU)
			return thet
		except Exception as e:
			print(type(e)) 
			print("[ERROR] ImageProcessor.threshold_grayscale: Ran into exception:", e)
			raise Exception
		

	def normalize_image(self, img):
		cv2.normalize(img, img, 0, 255, norm_type=cv2.NORM_MINMAX)
		return img

	def get_bgr_index(self):
		# This method should be removed when the UI is finished...
		max_value_array = self.config['filter values']['max']
		max = 0
		return_index = 0
		current_index = 0 
		for value in max_value_array:
			if value > max:
				max = value
				return_index = current_index
			current_index += 1
		return return_index


	def update_config(self, new_config):
		self.config = new_config

	def update_mode(self, new_mode):
		self.mode = new_mode

	def test_show_img(self, img, title='image'):
		cv2.imshow(title, img)
		cv2.waitKey(0)
		cv2.destroyAllWindows()
