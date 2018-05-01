import tensorflow as tf
from tensorflow.examples.tutorials.mnist import input_data
mnast = input_data.read_data_sets("MNIST_data/", one_hot=True)

# Python optimisation variables
learning_rate = 0.0001
epochs = 10
batch_size = 50 

#declare the training data placeholders 
# input x - for 28 x 28 pixels = 784 - this is the flattened 
# image data that is drawn from mnist.train.nextbatch()

x = tf.placeholder(tf.float32, [None, 784])

# dynamically reshape the input
x_shaped = tf.reshape(x, [-1, 28, 28, 1])

# now declare the output data placeholder - 10 digits 
y = tf.placeholder(tf.float32, [None, 10])


