import numpy as np
import cv2
import os



def compile_tif_frames(directory, frame_file_list):
	if not len(frame_file_list) > 0:
		print('[ERROR] Combiner: frame_file_list is empty')
		return
	else:
		images = [cv2.imread(directory + filename) for filename in frame_file_list]
		print(len(images))
		height, width = images[0].shape[:2]
		result = np.zeros((height, width, 3), np.uint8)
		for image in images:
			result = cv2.addWeighted(result, 1.0, image, 0.75, 0)
		cv2.imshow('oh man', result)
		cv2.waitKey(0)
		cv2.destroyAllWindows()


if __name__ == '__main__':
	directory = 'test_dir/'
	file_name_list = [filename for subdir, dirs, filename in os.walk(directory)][0]
	compile_tif_frames(directory, file_name_list)