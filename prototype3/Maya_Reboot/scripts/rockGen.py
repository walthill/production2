import maya.cmds as cmds
import random

import maya.cmds as cmds


def UIwindow():
    if cmds.window("UIwindow", exists=True):
        cmds.deleteUI("UIwindow", window=True)

    if cmds.windowPref("UIwindow", exists=True):
        cmds.windowPref('UIwindow', remove=True)

    cmds.window("UIwindow", title="Create Building", minimizeButton=False, maximizeButton=False, sizeable=False)

    cmds.columnLayout("columnMain_C", parent="UIwindow")

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 250)], parent="columnMain_C")
    cmds.button(label='Make a Rock', height = 40, command=lambda args: makeRocks())

    cmds.showWindow("UIwindow")
    # dont forget this!!!! and put it at end


def makeRocks():
    wANDd = 75
    loopNumList = list(range(3, 10))
    lengthList = list(range(10, 50))
    scaleList = [0.75, 0.85, 0.95, 1.0, 1.05, 1.15, 1.25]
    decideList = [True, False]

    randomNum = random.choice(loopNumList)
    rock = cmds.polyCube(w=wANDd, h=wANDd, d=wANDd, ch=False, name="rock_#")
    y=0
    z=0
    for x in range(randomNum):
        list_1 = list(range(0, 11 + z))
        list_2 = list(range(0, 5 + y))
        moveIt = random.choice(lengthList)
        scaleIt = random.choice(scaleList)
        optionIt = random.choice(list_1)
        optionIt = str(optionIt)
        optionIt = ".e[" + optionIt + "]"
        scaleOP = random.choice(list_2)
        scaleOP = str(scaleOP)
        scaleOP = ".f[" + scaleOP + "]"
        decisionY = random.choice(decideList)
        decisionX = random.choice(decideList)
        decisionZ = random.choice(decideList)
        cmds.move(moveIt, optionIt, relative=True, moveY=decisionY, moveX=decisionX, moveZ=decisionZ)
        cmds.scale(scaleIt, scaleOP, relative=True, scaleY=decisionY, scaleX=decisionX, scaleZ=decisionZ)
        y+=3
        z+=8
    cmds.polyBevel(rock, o=15)