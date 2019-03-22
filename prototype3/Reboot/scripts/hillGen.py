import maya.cmds as cmds
import random

def hillsUI():
    if cmds.window("UITextwindow", exists=True):
        cmds.deleteUI("UITextwindow", window=True)

    if cmds.windowPref("UITextwindow", exists=True):
        cmds.windowPref('UITextwindow', remove=True)

    cmds.window("UITextwindow", title="Terrain Generator", minimizeButton=False, maximizeButton=False, sizeable=False)
    columnWidth = 150.0
    nothing = ""

    cmds.columnLayout("columnMain_A", parent="UITextwindow")

    cmds.rowColumnLayout(numberOfColumns=2, columnWidth=[(1, columnWidth/2), (2, columnWidth/2)], parent="columnMain_A")
    cmds.text("Min Height")
    cmds.intField("minHill", min=15, max=75, step=1)
    cmds.text("Max Height")
    cmds.intField("maxHill", min=20, max=75, step=1)
    cmds.text("Hill Amount")
    cmds.intSlider('loopNum', minValue=5, maxValue=40)
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.button(label='Make Some Hills', height=30, bgc=(1.0, 1.0, 0.4), command=lambda args: makeHills())

    #end line
    cmds.showWindow("UITextwindow")


def makeHills():
    minHill = cmds.intField('minHill', query=True, value=True)
    maxHill = cmds.intField('maxHill', query=True, value=True)
    loopNum = cmds.intSlider('loopNum', query=True, value=True)
    faceList = list(range(0,624))
    moveList = list(range(minHill,maxHill))
    surface = cmds.polyPlane(w=500, h=500, sx=25, sy=25, ch=False)
    surface = surface[0]

    cmds.softSelect(sse=1)
    cmds.softSelect(sse=1, ssd=100.0, ssc='0,1,2,1,0,2', ssf=2)

    if minHill == maxHill:
        cmds.warning("Min and Max were the same")
        for x in range(loopNum):
            chosenFace = random.choice(faceList)
            chosenFace = str(chosenFace)
            chosenFace = ".f[" + chosenFace + "]"
            selectedFace = cmds.select(surface + chosenFace)
            moveInY = maxHill
            cmds.move(moveInY, selectedFace, relative=True, moveY=True)
    if minHill > maxHill:
        cmds.delete(surface)
        cmds.error("Min Hill Height was greater than Max Hill Height. Fix and try again.")
    else:
        for x in range(loopNum):
            chosenFace = random.choice(faceList)
            chosenFace = str(chosenFace)
            chosenFace = ".f[" + chosenFace + "]"
            moveInY = random.choice(moveList)
            selectedFace = cmds.select(surface + chosenFace)
            cmds.move(moveInY, selectedFace, relative=True, moveY=True)

    cmds.select(clear=True)