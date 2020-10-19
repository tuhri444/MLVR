import os
import cv2
import random
import numpy as np
import tensorflow as tf

class DataReader:
    def __init__(self, data_path, categories, image_size):
        self.data = []
        self.x_train = []
        self.x_test = []
        self.y_train = []
        self.y_test = []
        training_data = self._create_training_data(data_path, categories, image_size)
        x_list, y_list = self._split_training_data(training_data, image_size)
        self.x_train, self.y_train, self.x_test, self.y_test = self._segment_training_testing(x_list, y_list, 20)
        self.y_train = tf.keras.utils.to_categorical(self.y_train)

    def _create_training_data(self, data_path, categories, image_size):
        output = []
        for category in categories:
            path = os.path.join(data_path, category)  # path to each folder
            print("Loading Files from path: \n", path, "\n")
            class_num = categories.index(category)  # get numerical index of category
            for img in os.listdir(path):
                try:
                    # read in data by combining path with img name and turning it grayscale
                    img_array = cv2.imread(os.path.join(path, img), cv2.IMREAD_GRAYSCALE)
                    new_array = cv2.resize(img_array, (image_size, image_size))
                    # adding recolored and resized image to training data array
                    output.append([new_array, class_num])
                except Exception as e:
                    pass
        random.shuffle(output)  # shuffle the data
        return output

    def _split_training_data(self, training_data, image_size):
        x_list = []
        y_list = []

        # split training data, creating a list of features and labels
        for features, label in training_data:
            x_list.append(features)
            y_list.append(label)

        # resize shape of array
        x_list = np.array(x_list).reshape(-1, image_size, image_size, 1)

        return x_list, y_list

    def _segment_training_testing(self, a_list, b_list, percent_for_testing):
        testing_amount = int(10/100*percent_for_testing)

        a_train = []
        b_train = []
        a_test = []
        b_test = []

        a_list = np.array_split(np.array(a_list / 255.0), 10)
        b_list = np.array_split(np.array(b_list), 10)

        for segment in a_list:
            segment = np.array(segment)
        for segment in b_list:
            segment = np.array(segment)

        for i in range(10):
            if i < 10-testing_amount:
                for segment in a_list[i]:
                    a_train.append(segment)
                for segment in b_list[i]:
                    b_train.append(segment)
            else:
                for segment in a_list[i]:
                    a_test.append(segment)
                for segment in b_list[i]:
                    b_test.append(segment)

        a_train = np.array(a_train)
        b_train = np.array(b_train)
        a_test = np.array(a_test)
        b_test = np.array(b_test)

        return a_train, b_train, a_test, b_test
