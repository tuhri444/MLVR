from DataReader import DataReader
from MLTraining import MLTraining
import tensorflow as tf
import os
import sys
import tf2onnx
from tf2onnx.tfonnx import process_tf_graph
import tf2onnx.optimizer as optimizer
import tf2onnx.utils as utils
from tf2onnx import tf_loader
from tensorflow import keras
# import matplotlib.pyplot as plt

def ConvertToONNX():
    graph, ins, outs = tf2onnx.tf_loader.from_saved_model("model", [], [])

    with tf.Graph().as_default() as tf_graph:
        tf.import_graph_def(graph, name='')

    with tf_loader.tf_session(graph=tf_graph):
        g = process_tf_graph(tf_graph,
                             input_names=ins,
                             output_names=outs,)

    onnx_graph = optimizer.optimize_graph(g)
    model_proto = onnx_graph.make_model("converted from {}".format('output/model.onnx'))

    utils.save_protobuf('output/model.onnx', model_proto)

if __name__ == '__main__':
    datapath = "DataSet/"
    categories = os.listdir(datapath)
    image_size = 64
    if len(sys.argv) == 2:
        image_size = int(sys.argv[1])

    datareader = DataReader(datapath, categories, image_size)
    ai = MLTraining(datareader.x_train.shape[1:], image_size, categories)

    ai.set_x_train(datareader.x_train)
    ai.set_y_train(datareader.y_train)

    ai.train_model()

    ConvertToONNX()

    # This is for debugging, don't touch if you want everything to run like normal ;)
    # prediction = ai.predict(datareader.x_test)
    #
    # correct = 0
    # amountToTest = len(datareader.x_test)
    # for i in range(amountToTest):
    #     highestValue = 0
    #     index = 0
    #     highestValueIndex = 0
    #     for value in prediction[i]:
    #         print(value)
    #         if value > highestValue:
    #             highestValue = value
    #             highestValueIndex = index
    #         index += 1
    #
    #     if categories[highestValueIndex] == categories[datareader.y_test[i]]:
    #         correct += 1
    #     plt.grid(False)
    #     plt.imshow(tf.squeeze(datareader.x_test[i]))
    #     plt.xlabel("Actual: " + categories[datareader.y_test[i]])
    #     plt.title("Prediction: " + categories[highestValueIndex])
    #     plt.show()
    #
    # print("Predicted ", correct, " out of ", amountToTest, " right")
    # percentage = 100/len(datareader.x_test)*correct
    # print(percentage)