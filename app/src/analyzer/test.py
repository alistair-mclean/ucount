import cv2
import numpy as np

def run():
	path ='~/research/uCounter/web_app/analyzer' 
	fileName = 'Mix_Well1_2Steel_new.tif'
	img = cv2.imread(fileName)
	original = img


	b, g, r = cv2.split(img)
	kSize = 3
	gBlur = cv2.blur(g, (kSize, kSize))
	clahe = cv2.createCLAHE(clipLimit=1.5, tileGridSize=(3,3))
	gClaheBlur = clahe.apply(gBlur)
	gEdges = cv2.Laplacian(gBlur, cv2.CV_8UC1)
	gClaheEdges = clahe.apply(gEdges)


	circles = cv2.HoughCircles(gClaheEdges, cv2.HOUGH_GRADIENT, 8, 10, param1=50,param2=60,minRadius=5,maxRadius=10)
	circles = np.uint16(np.around(circles))
	for i in circles[0,:]:
	    # draw the outer circle
	    cv2.circle(img,(i[0],i[1]),i[2],(255,255,0),1)
	    # draw the center of the circle
	    #cv2.circle(img,(i[0],i[1]),2,(0,0,255),3)

	cv2.imshow('gClaheEdges',gClaheEdges)
	cv2.imshow('detected circles',img)
	cv2.waitKey(0)
	cv2.destroyAllWindows()

if __name__ == '__main__':
	run()