import cv2
import numpy as np
import matplotlib
import matplotlib.pyplot as plt


class PostProcessor(): 
	def test(self, img):
		gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
		laplacian = cv2.Laplacian(img, cv2.CV_64F)
		
		sobelx = cv2.Sobel(img, cv2.CV_64F, 1, 0, ksize=5)
		sobely = cv2.Sobel(img, cv2.CV_64F, 0, 1, ksize=5)

		plt.subplot(2,2,1),plt.imshow(img,cmap='gray')
		plt.title('Original'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,2),plt.imshow(laplacian,cmap = 'gray')
		plt.title('Laplacian'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,3),plt.imshow(sobelx,cmap = 'gray')
		plt.title('Sobel X'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,4),plt.imshow(sobely,cmap = 'gray')
		plt.title('Sobel Y'), plt.xticks([]), plt.yticks([])


		plt.show()

	# Apply a laplacian convolution 
	def SharpenEdges(self, img):
		return cv2.Laplacian(img, cv2.CV_64F)

	# Apply a gaussian blur
	def Smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))

	
class PreProcessor(): 
	def test(self, img):
		gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
		laplacian = cv2.Laplacian(img, cv2.CV_64F)
		
		sobelx = cv2.Sobel(img, cv2.CV_64F, 1, 0, ksize=5)
		sobely = cv2.Sobel(img, cv2.CV_64F, 0, 1, ksize=5)

		plt.subplot(2,2,1),plt.imshow(img,cmap='gray')
		plt.title('Original'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,2),plt.imshow(laplacian,cmap = 'gray')
		plt.title('Laplacian'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,3),plt.imshow(sobelx,cmap = 'gray')
		plt.title('Sobel X'), plt.xticks([]), plt.yticks([])
		plt.subplot(2,2,4),plt.imshow(sobely,cmap = 'gray')
		plt.title('Sobel Y'), plt.xticks([]), plt.yticks([])


		plt.show()


# Apply a laplacian convolution 
	def SharpenEdges(self, img):
		return cv2.Laplacian(img, cv2.CV_64F)

	# Apply a gaussian blur
	def Smooth(self, img, ksize):
		return cv2.blur(img, (ksize, ksize))



