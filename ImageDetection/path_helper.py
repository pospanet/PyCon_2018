# Copyright (c) Microsoft. All rights reserved.

# Licensed under the MIT license. See LICENSE.md file in the project root
# for full license information.
# author: janpos@microsoft.com
# ==============================================================================

import os

class UserDirectory:
    
    def __init__(self, projectName, projectsDir = "Projects", datasetDir = "dataset"):
        self.projectName = projectName
        self.projectsDirectory = projectsDir
        self.datasetDirectory = datasetDir
    
    def getImageDir(self, imageSubdir):

        # Loged-in user
        windowsUser = os.getlogin()

        # User folder
        userDir = os.path.join("C:/Users", windowsUser)

        # Path for Images and boxe
        imgDir = os.path.join(userDir, "Desktop", self.projectName, self.datasetDirectory, imageSubdir)
        if not os.path.isdir(imgDir):
            imgDir = os.path.join(userDir, self.projectsDirectory, self.projectName, self.datasetDirectory, imageSubdir)
        return imgDir

def getFilesInDirectory(directory, postfix = ""):
    fileNames = [s for s in os.listdir(directory) if not os.path.isdir(os.path.join(directory, s))]
    if not postfix or postfix == "":
        pass
    else:
        fileNames = [s for s in fileNames if s.lower().endswith(postfix)]
    return fileNames

def getFilesInDirectoryByType(directory, fileTypes):
    files = []
    for fileType in fileTypes:
        files += getFilesInDirectory(directory, "." + fileType)
    return files


####################################
# Functions
####################################


def readFile(inputFile):
    with open(inputFile,'rb') as f:
        lines = f.readlines()
    return [removeLineEndCharacters(s) for s in lines]



def readTable(inputFile, delimiter='\t', columnsToKeep=None):
    lines = readFile(inputFile)
    if columnsToKeep != None:
        header = lines[0].split(delimiter)
        columnsToKeepIndices = listFindItems(header, columnsToKeep)
    else:
        columnsToKeepIndices = None
    return splitStrings(lines, delimiter, columnsToKeepIndices)


def removeLineEndCharacters(line):
    if line.endswith(b'\r\n'):
        return line[:-2]
    elif line.endswith(b'\n'):
        return line[:-1]
    else:
        return line


def splitString(string, delimiter='\t', columnsToKeepIndices=None):
    if string == None:
        return None
    items = string.decode('utf-8').split(delimiter)
    if columnsToKeepIndices != None:
        items = getColumns([items], columnsToKeepIndices)
        items = items[0]
    return items

def splitStrings(strings, delimiter, columnsToKeepIndices=None):
    table = [splitString(string, delimiter, columnsToKeepIndices) for string in strings]
    return table

def writeFile(outputFile, lines):
    with open(outputFile,'w') as f:
        for line in lines:
            f.write("%s\n" % line)

def writeTable(outputFile, table):
    lines = tableToList1D(table)
    writeFile(outputFile, lines)


def tableToList1D(table, delimiter='\t'):
    return [delimiter.join([str(s) for s in row]) for row in table]