# Copyright (c) Microsoft. All rights reserved.

# Licensed under the MIT license. See LICENSE.md file in the project root
# for full license information.
# author: janpos@microsoft.com
# ==============================================================================

from __future__ import print_function

from data_helper import *

import os, sys

from download_model import download_model_by_name
# Parameters

cleanup_trained_data = True

def download_model(model_file_name, model_url):
    model_dir = os.path.join(os.path.dirname(os.path.abspath(__file__)), "PretrainedModels")
    filename = os.path.join(model_dir, model_file_name)
    if not os.path.exists(filename):
        print('Downloading model from ' + model_url + ', may take a while...')
        urlretrieve(model_url, filename)
        print('Saved model as ' + filename)
    else:
        print('CNTK model already available at ' + filename)
    
print(" !!! Generating mappings !!! STARTED")    
create_mappings()
print(" !!! Generating mappings !!! FINISHED")    

print(" !!! DOwnloading model !!! STARTED")    
download_model_by_name("AlexNet_ImageNet_Caffe")
print(" !!! DOwnloading model !!! FINISHED")    
# download_model_by_name("AlexNet")


if cleanup_trained_data:
    print(" !!! Cleaning Output !!! STARTED")    
    fileList = [ f for f in os.listdir("Output") if f.endswith(".model") ]
    for fileName in fileList:
        modelFileName = os.path.join("Output", fileName)
        print("Deleting already trained " + modelFileName)
        os.remove(modelFileName)
    print(" !!! Cleaning Output !!! FINISHED")    