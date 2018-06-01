# Copyright (c) Microsoft. All rights reserved.

# Licensed under the MIT license. See LICENSE.md file in the project root
# for full license information.
# modified by janpos@microsoft.com
# ==============================================================================

import numpy as np
import os
from path_helper import *

img_width  = 788
img_height = 788

class Config(object):
    def __init__(self):
        pass

def _get_image_paths(img_dir, training_set):
    if training_set:
        subDirs = ['train']
    else:
        subDirs = ['test']

    image_paths = []
    for subdir in subDirs:
        sub_dir_path = os.path.join(img_dir, subdir)
        imgFilenames = getFilesInDirectoryByType(sub_dir_path, ["jpg", "png"])
        for img in imgFilenames:
            image_paths.append("{}/{}".format(subdir, img))

    return image_paths

def _removeLineEndCharacters(line):
    if line.endswith(b'\r\n'):
        return line[:-2]
    elif line.endswith(b'\n'):
        return line[:-1]
    else:
        return line

def _load_annotation(imgPath):
    bboxesPaths = imgPath[:-4] + ".bboxes.tsv"
    labelsPaths = imgPath[:-4] + ".bboxes.labels.tsv"
    # if no ground truth annotations are available, return None
    if not os.path.exists(bboxesPaths) or not os.path.exists(labelsPaths):
        return None
    bboxes = np.loadtxt(bboxesPaths, np.int32)

    # in case there's only one annotation and numpy read the array as single array,
    # we need to make sure the input is treated as a multi dimensional array instead of a list/ 1D array
    if len(bboxes.shape) == 1:
        bboxes = np.array([bboxes])

    with open(labelsPaths, 'rb') as f:
        lines = f.readlines()
    labels = [_removeLineEndCharacters(s) for s in lines]

    labels = np.asarray(labels)
    labels.shape = labels.shape + (1,)

    annotations = np.hstack((bboxes, labels))

    return annotations

def create_mappings():
    abs_path = os.path.dirname(os.path.abspath(__file__))
    data_set_path = os.path.abspath(os.path.join(abs_path, "./images"))
    create_map_files(data_set_path, training_set=False)
    create_map_files(data_set_path, training_set=True)

def create_map_files(data_folder, training_set):
    # get relative paths for map files
    img_file_paths = _get_image_paths(data_folder, training_set)
    totalImageCount = len(img_file_paths)

    roi_file_path = os.path.join(data_folder, "{}_labels.csv".format("train" if training_set else "test"))

    print("... CRATING MAPPING ...")
    counter = 0
    with open(roi_file_path, 'w') as roi_file:
        roi_file.write("filename,width,height,class,xmin,ymin,xmax,ymax" + "\n")
        for img_path in img_file_paths:
            abs_img_path = os.path.join(data_folder, img_path)
            gt_annotations = _load_annotation(abs_img_path)
            if gt_annotations is None:
                continue

            for box in gt_annotations:
                roi_line = "{},{},{},{},{},{},{},{}\n".format(
                    os.path.basename(img_path), 
                    img_width, 
                    img_height, 
                    box[4].decode('utf-8'), 
                    box[0].decode('utf-8'), 
                    box[1].decode('utf-8'), 
                    box[2].decode('utf-8'), 
                    box[3].decode('utf-8'))
                roi_file.write(roi_line)
            counter += 1
            if counter % 100 == 0:
                print("Processed {} images out of {}".format(counter, totalImageCount))


if __name__ == '__main__':
    parameters = Config()
    parameters.use()
