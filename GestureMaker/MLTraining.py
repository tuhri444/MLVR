import tensorflow as tf
from tensorflow import keras
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Dense, Activation, Flatten, Conv2D, MaxPooling2D
import numpy as np

class MLTraining:
    def __init__(self, input_shape, image_size, categories):
        self.data = []
        self.model = Sequential()
        self._x_train = []
        self._y_train = []
        self._initialize_model(input_shape, image_size, categories)

    def _initialize_model(self, input_shape, image_size, categories):
        # layer 1
        self.model.add(Conv2D(image_size, (4, 4), input_shape=input_shape))
        self.model.add(Activation("relu"))
        self.model.add(MaxPooling2D(pool_size=(3, 3)))

        # layer 2
        self.model.add(Conv2D(image_size, (4, 4)))
        self.model.add(Activation("relu"))
        self.model.add(MaxPooling2D(pool_size=(3, 3)))

        # layer 3
        self.model.add(Conv2D(image_size, (3, 3)))
        self.model.add(Activation("relu"))
        self.model.add(MaxPooling2D(pool_size=(3, 3)))

        # layer 4
        self.model.add(Flatten())
        self.model.add(Dense(image_size))

        # output layer
        self.model.add(Dense(len(categories)))
        self.model.add(Activation('softmax'))

        self.model.compile(loss="categorical_crossentropy",
                           optimizer="adam",
                           metrics=['accuracy'])

    def set_x_train(self, x):
        self._x_train = x

    def set_y_train(self, y):
        self._y_train = y

    def train_model(self):
        # train model
        self.model.fit(self._x_train, self._y_train, batch_size=32, epochs=10)
        # save model
        self.model.save("model")

    def predict(self, test_data):
        return self.model.predict(test_data)