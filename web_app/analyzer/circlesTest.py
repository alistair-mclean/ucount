import cv2
import numpy as np

#img = cv2.imread('opencv-logo.png',0)
img = cv2.imread('Mix_Well1_2Steel_new.tif',0)

def test1():
	img = cv2.imread('Mix_Well1_2Steel_new.tif',0)
	img = cv2.medianBlur(img,5)
	cimg = cv2.cvtColor(img,cv2.COLOR_GRAY2BGR)

	circles = cv2.HoughCircles(img,cv2.HOUGH_GRADIENT,1,20,
	                            param1=50,param2=30,minRadius=0,maxRadius=0)

	circles = np.uint16(np.around(circles))
	for i in circles[0,:]:
	    # draw the outer circle
	    cv2.circle(cimg,(i[0],i[1]),i[2],(0,255,0),2)
	    # draw the center of the circle
	    cv2.circle(cimg,(i[0],i[1]),2,(0,0,255),3)

	cv2.imshow('detected circles',cimg)
	cv2.waitKey(0)
	cv2.destroyAllWindows()


def test2():
	img = cv2.imread('Mix_Well1_2Steel_new.tif',0)
	grays = cv2.split(img)
	#grays = self.channelsToGrayScale(channels)
	indx = 0
	clahe = cv2.createCLAHE(clipLimit=3.0, tileGridSize=(8,8))
	for channel in grays:
		channel = cv2.blur(channel, (3,3))
		channel = clahe.apply(channel)
		grays[indx] = channel
		indx += 1
	print(len(grays))
	cimg = cv2.cvtColor(grays[1],cv2.COLOR_GRAY2BGR)

	circles = cv2.HoughCircles(grays[1],cv2.HOUGH_GRADIENT,1,20,
	                            param1=50,param2=30,minRadius=0,maxRadius=0)

	circles = np.uint16(np.around(circles))
	for i in circles[0,:]:
	    # draw the outer circle
	    cv2.circle(cimg,(i[0],i[1]),i[2],(0,255,0),2)
	    # draw the center of the circle
	    cv2.circle(cimg,(i[0],i[1]),2,(0,0,255),3)

if __name__ == '__main__':
	test1()
	test2()