# Copyright (c) Microsoft. All rights reserved.

# Licensed under the MIT license. See LICENSE.md file in the project root
# for full license information.
# author: janpos@microsoft.com
# ==============================================================================

import path_helper as ph
import os

def getNumberOfTrainImages(datasetFolder, subfolder = "positive"):
    return getNumnerOfFiles(datasetFolder, subfolder, ["bboxes.tsv"])

def getNumberOfTestImages(datasetFolder, subfolder = "testImages"):
    return getNumnerOfFiles(datasetFolder, subfolder, ["bboxes.tsv"])

def getNumnerOfFiles(folder, subfolder, suffix):
    folder = os.path.join(folder, subfolder)
    files = ph.getFilesInDirectoryByType(folder, suffix)
    fileCount = len(files)
    return fileCount    
