import maya.cmds as cmds
import random


"""
What to copy and paste:

import biomeGen
reload(biomeGen)
biomeGen.rebootGenUI()
"""


def rebootGenUI():
    if cmds.window("UIwindow", exists=True):
        cmds.deleteUI("UIwindow", window=True)

    if cmds.windowPref("UIwindow", exists=True):
        cmds.windowPref('UIwindow', remove=True)

    cmds.window("UIwindow", title="Terrain Generator", minimizeButton=False, maximizeButton=False, sizeable=False)
    columnWidth = 150.0
    nothing = ""

    cmds.columnLayout("columnMain_A", parent="UIwindow")

    #SECTION ONE: HILLS
    cmds.rowColumnLayout(numberOfColumns=2, columnWidth=[(1, columnWidth / 2), (2, columnWidth / 2)],
                         parent="columnMain_A")
    cmds.text("Min Height")
    cmds.intField("minHill", min=15, max=75, step=1)
    cmds.text("Max Height")
    cmds.intField("maxHill", min=20, max=75, step=1)
    cmds.text("Hill Amount")
    cmds.intSlider('loopNum', minValue=5, maxValue=40)
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.button(label='Make Some Hills', height=30, bgc=(1.0, 1.0, 0.4), command=lambda args: makeHills())

    #SECTION TWO: CITY
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.separator(height=10, style="in")
    cmds.checkBox("geomAdd", label='Add Geometry')
    cmds.checkBox("geomSub", label='Sub Geometry')
    cmds.rowColumnLayout(numberOfColumns=2, columnWidth=[(1, columnWidth / 2), (2, columnWidth / 2)],
                         parent="columnMain_A")
    cmds.text("Min Height")
    cmds.intField("bMin", min=275, max=1000, step=1)
    cmds.text("Max Height")
    cmds.intField("bMax", min=300, max=1500, step=1)
    cmds.text("City Size")
    cmds.intSlider('bAmount', minValue=4, maxValue=100, step=2)
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.button(label='Make Some Buildings', height=30, bgc=(1.0, 1.0, 0.4), command=lambda args: makeCity())

    #end line
    cmds.showWindow("UIwindow")


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

    cmds.softSelect(sse=0)
    cmds.select(clear=True)


def makeCity():
    bMin = cmds.intField('bMin', query=True, value=True)
    bMax = cmds.intField('bMax', query=True, value=True)
    geomAdd = cmds.checkBox('geomAdd', query=True, value=True)
    geomSub = cmds.checkBox('geomSub', query=True, value=True)
    bAmount = cmds.intSlider('bAmount', query=True, value=True)
    randomList = list(range(0,10))
    moveList = list(range(20, 50))
    rotateList = list(range(0, 360))
    spacing = 200
    topMove = 10
    bWidth = 50
    divisionNum = 0
    if bAmount >= 20:
        divisionNum = 4
    if bAmount < 20:
        divisionNum = 2
    if bAmount >= 60:
        divisionNum = 8
    buildList= []

    cmds.softSelect(sse=0)

    for x in range(divisionNum):
        for y in range(bAmount/divisionNum):
            build = buiilding(bMin, bMax, bWidth)
            cmds.move(spacing * y, 0, spacing*x, build, relative=True)
            buildList.append(build)
            cmds.delete('.f[3]')
            chosenNum = random.choice(randomList)
            thisGotGeom = False
            if geomAdd == True and chosenNum >= 8:
                thisGotGeom = True
                faceList = ['.f[0]', '.f[2]', '.f[3]', '.f[4]']
                scaleList = list(range(10, 60))
                loopRandom = random.choice(randomList)
                for z in range(loopRandom/4):
                    chosenFace = random.choice(faceList)
                    scaleNum_1 = random.choice(scaleList)/100.0
                    scaleNum_2 = random.choice(scaleList)/100.0
                    moveNum_1 = random.choice(moveList)
                    cmds.polyExtrudeFacet(chosenFace, lsx=scaleNum_1*2, lsy=scaleNum_2)
                    cmds.polyExtrudeFacet(chosenFace, ltz=moveNum_1)
                    faceList = ['.f[9]', '.f[11]']
            cmds.polyExtrudeFacet('.f[1]', lsx=1.1, lsy=1.1)
            cmds.polyExtrudeFacet('.f[1]', ltz=topMove)
            cmds.polyExtrudeFacet('.f[1]', lsx=0.8, lsy=0.8)
            cmds.polyExtrudeFacet('.f[1]', ltz=-topMove/2.0)
            chosenNum = random.choice(randomList)
            if geomSub == True and chosenNum >= 8 and thisGotGeom != True:
                heightTemp = random.choice(moveList)
                moveTemp = random.choice(moveList)
                moveTemp_2 = random.choice(moveList)
                rotateTemp = random.choice(rotateList)
                tempObject = cmds.polyCylinder(r=bWidth*4, h=heightTemp, ch=False)
                cmds.rotate(90, tempObject, relative=True)
                cmds.rotate(rotateTemp, tempObject, relative=True)
                cmds.move(spacing * y, heightTemp/2.0+(bMin-moveTemp-moveTemp_2), spacing*x, tempObject, relative=True)
                build = cmds.polyCBoolOp(build, tempObject, operation=2, ch=False)
                cmds.polyCloseBorder(build)
            cmds.polyTriangulate(build)
            cmds.polyQuad(build)

    cmds.select(clear=True)


def buiilding(bMin, bMax, bWidth):
    try:
        heightList = list(range(bMin, bMax))
        bHeight = random.choice(heightList)
        build = cmds.polyCube(w=bWidth, h=bHeight, d=bWidth, ch=False)
        cmds.move(bHeight/2.0, build, relative=True, moveY=True)
        return build
    except:
        cmds.error("Minimum Height was greater than Maximum Height or Minimum and Maximum Heights were the same. "
                   "Fix and try again.")