import maya.cmds as cmds
import random
import sys


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

    # SECTION THREE: CITY SIGN POSTS
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.button(label='Make A Sign', height=30, bgc=(1.0, 1.0, 0.4), command=lambda args: signpost())

    # SECTION FOUR: TRASH
    cmds.rowColumnLayout(numberOfColumns=2, columnWidth=[(1, columnWidth / 2), (2, columnWidth / 2)],
                         parent="columnMain_A")
    cmds.text("Min Height")
    cmds.intField("tMin", min=10, max=100, step=1)
    cmds.text("Max Height")
    cmds.intField("tMax", min=50, max=200, step=1)
    cmds.text("Trash Amount")
    cmds.intSlider('tNum', minValue=1, maxValue=20, step=1)
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, columnWidth)], parent="columnMain_A")
    cmds.button(label='Make Some Trash', height=30, bgc=(1.0, 1.0, 0.4), command=lambda args: makeTrash())

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
    seed = random.randrange(sys.maxsize)
    random.Random(seed)
    sizeList = list(range(15, 50))
    randLocScale = list(range(60, 120))
    bMin = cmds.intField('bMin', query=True, value=True)
    bMax = cmds.intField('bMax', query=True, value=True)
    geomAdd = cmds.checkBox('geomAdd', query=True, value=True)
    geomSub = cmds.checkBox('geomSub', query=True, value=True)
    bAmount = cmds.intSlider('bAmount', query=True, value=True)
    randomList = list(range(0,10))
    moveList = list(range(20, 50))
    addMoveList = list(range(-20, 20))
    rotateList = list(range(-45, 45))
    negORpos = [-1, 1]
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
    buildGroup = cmds.group(empty=True, world=True, name="buildGroup_#")

    cmds.softSelect(sse=0)

    for x in range(divisionNum):
        for y in range(bAmount/divisionNum):
            addedMovement = random.choice(addMoveList)
            build = building(bMin, bMax, bWidth)
            cmds.move((spacing + addedMovement) * y, 0, (spacing + addedMovement)*x, build, relative=True)
            buildList.append(build)
            cmds.delete('.f[3]')
            chosenNum = random.choice(randomList)
            thisGotGeom = False
            thisSubGeom = False
            cmds.polyExtrudeFacet('.f[1]', lsx=1.1, lsy=1.1)
            cmds.polyExtrudeFacet('.f[1]', ltz=topMove)
            cmds.polyExtrudeFacet('.f[1]', lsx=0.8, lsy=0.8)
            cmds.polyExtrudeFacet('.f[1]', ltz=-topMove / 2.0, lsx=0.6, lsy=0.6)
            if geomAdd == True and chosenNum >= 8:
                thisGotGeom = True
                loopRandom = random.choice(randomList)
                faceList = ['.f[0]', '.f[2]', '.f[3]', '.f[4]']
                for z in range(loopRandom / 4):
                    chosenFace = random.choice(faceList)
                    cmds.select(chosenFace)
                    X, Y, Z = cmds.polyEvaluate(bc=True)
                    x1, x2 = X
                    y1, y2 = Y
                    z1, z2 = Z
                    xTotal = (x1 + x2) / 2
                    yTotal = (y1 + y2) / 2
                    zTotal = (z1 + z2) / 2
                    cmds.select(clear=True)
                    object = buildAddOns(sizeList, randLocScale)
                    object = object[0]
                    cmds.move(xTotal, yTotal, zTotal, object, relative=True)
                    faceList.remove(chosenFace)
                    build = cmds.polyCBoolOp(build, object, operation = 1, ch=False)
            chosenNum = random.choice(randomList)
            if geomSub == True and chosenNum >= 7 and thisGotGeom != True:
                thisSubGeom = True
                moveTemp = random.choice(moveList)
                moveTemp_2 = random.choice(moveList)
                moveTemp_3 = random.choice(moveList)
                rotateTemp = random.choice(rotateList)
                tempObject = buildSubtraction(bWidth, sizeList, randLocScale)
                cmds.rotate(rotateTemp, tempObject, relative=True)
                cmds.move((spacing + addedMovement) * y, (bMin-moveTemp-moveTemp_2- moveTemp_3),
                          (spacing + addedMovement)*x, tempObject, relative=True)
                build = cmds.polyCBoolOp(build, tempObject, operation=2, ch=False)
                cmds.polyCloseBorder(build)
            chosenNum = random.choice(randomList)
            if chosenNum >= 8 and thisGotGeom != True and thisSubGeom != True:
                chosenNum = random.choice(randomList)
                heightTemp_1 = random.choice(moveList)
                heightTemp_2 = random.choice(moveList)
                numberOne = random.choice(negORpos)
                if chosenNum >= 5:
                    beam = cmds.polyCube(w=bWidth, h=topMove, d=topMove, ch=False)
                    cmds.move((spacing + addedMovement) * y, topMove/2.0, (spacing + addedMovement) * x, beam,
                              relative=True)
                    cmds.move(0, heightTemp_1 + heightTemp_2, (bWidth/2.0 + topMove/2.0)*numberOne, beam, relative=True)
                if chosenNum < 5:
                    beam = cmds.polyCube(w=topMove, h=topMove, d=bWidth, ch=False)
                    cmds.move((spacing + addedMovement) * y, topMove / 2.0, (spacing + addedMovement) * x, beam,
                              relative=True)
                    cmds.move((bWidth / 2.0 + topMove / 2.0) * numberOne, heightTemp_1 + heightTemp_2, 0, beam,
                              relative=True)
                build = cmds.polyUnite(build, beam, ch=False)
            if x == 0 or y==0:
                cmds.delete(build)
                try:
                    cmds.delete(beam)
                except:
                    pass
            try:
                cmds.polyTriangulate(build)
                cmds.polyQuad(build)
                cmds.parent(build, buildGroup)
                cmds.rename(build, "building_#")
            except:
                pass

    cmds.select(clear=True)


def building(bMin, bMax, bWidth):
    try:
        heightList = list(range(bMin, bMax))
        bHeight = random.choice(heightList)
        build = cmds.polyCube(w=bWidth, h=bHeight, d=bWidth, ch=False)
        cmds.move(bHeight/2.0, build, relative=True, moveY=True)
        return build
    except:
        cmds.error("Minimum Height was greater than Maximum Height or Minimum and Maximum Heights were the same. "
                   "Fix and try again.")


def buildAddOns(sizeList, randLocScale):
    width = random.choice(sizeList)
    height = random.choice(sizeList)
    depth = random.choice(sizeList)
    cube = cmds.polyCube(w=width, h=height, d=depth, ch=False)
    faceList = ['.f[0]', '.f[1]', '.f[2]', '.f[3]', '.f[4]', '.f[5]']
    loopList = list(range(1, 4))
    loopNum = random.choice(loopList)

    for x in range(loopNum):
        randFace = random.choice(faceList)
        locScaleX = random.choice(randLocScale)
        locScaleY = random.choice(randLocScale)
        translateZ = random.choice(sizeList)
        cmds.polyExtrudeFacet(randFace, ltz=translateZ, lsx=locScaleX/100.0, lsy=locScaleY/100.0)
        if x == 0:
            faceList.append('.f[6]')
            faceList.append('.f[7]')
            faceList.append('.f[8]')
            faceList.append('.f[9]')
        if x == 1:
            faceList.append('.f[10]')
            faceList.append('.f[11]')
            faceList.append('.f[12]')
            faceList.append('.f[13]')
        if x == 2:
            faceList.append('.f[14]')
            faceList.append('.f[15]')
            faceList.append('.f[16]')
            faceList.append('.f[17]')
        if x == 1:
            faceList.append('.f[18]')
            faceList.append('.f[19]')
            faceList.append('.f[20]')
            faceList.append('.f[21]')
    return cube


def buildSubtraction(bWidth, sizeList, randLocScale):
    height = random.choice(sizeList)
    cube = cmds.polyCylinder(r=bWidth * 4, h=height, subdivisionsAxis=5, ch=False)
    faceList = ['.f[5]', '.f[6]']

    for x in range(2):
        randFace = random.choice(faceList)
        locScaleX = random.choice(randLocScale)
        locScaleY = random.choice(randLocScale)
        translateZ = random.choice(sizeList)
        cmds.polyExtrudeFacet(randFace, ltz=translateZ, lsx=locScaleX / 100.0, lsy=locScaleY / 100.0)
        if x == 0:
            faceList.append('.f[7]')
            faceList.append('.f[8]')
            faceList.append('.f[9]')
            faceList.append('.f[10]')
            faceList.append('.f[11]')

    return cube


def signpost():
    height = 10
    rad = 2
    linePoints = []
    curveAdjList = list(range(10, 25))
    smallAdj = list(range(-10, 10))
    plusORMinus = [-1, 1]

    for x in range(5):
        addedNum = random.choice(curveAdjList)
        Small_addedNum = random.choice(smallAdj)
        one = random.choice(plusORMinus)
        curveAdjList.remove(addedNum)
        if x == 0:
            addedNum = 12
            Small_addedNum = 0
        point_1 = x*addedNum/2*one + Small_addedNum, x * addedNum + addedNum, 0
        linePoints.append(point_1)

    extrudeCurve = cmds.curve(p=linePoints)
    rope = cmds.polyCylinder(r=rad, h=height, sx=3, ch=False)
    rope = rope[0]
    cmds.move(0, height / 2.0, 0, rope)
    cmds.delete('.f[3]')
    cmds.polyExtrudeFacet(rope + '.f[3]', inputCurve=extrudeCurve, divisions=12, ch=False)
    cmds.delete(extrudeCurve)
    cmds.polyExtrudeFacet('.f[0:2]', ltz=3)
    cmds.delete('.f[40]', '.f[42]', '.f[44]')
    cmds.polyBevel('.e[3:26]', '.e[39:50]', '.e[76:78]', '.e[80]', '.e[82:83]', o=1.2)
    cmds.select('.f[0]')
    X, Y, Z = cmds.polyEvaluate(bc=True)
    x1, x2 = X
    y1, y2 = Y
    z1, z2 = Z
    xTotal = (x1 + x2) / 2
    yTotal = (y1 + y2) / 2
    zTotal = (z1 + z2) / 2
    cmds.select(clear=True)
    object, object_2 = signTops()
    object = object[0]
    object_2 = object_2[0]
    cmds.move(xTotal, yTotal-5, zTotal, object, relative=True)
    cmds.move(xTotal, yTotal-5, zTotal, object_2, relative=True)
    rope = cmds.polyCBoolOp(rope, object, operation=2, ch=False)
    cmds.polyCBoolOp(object_2, rope, operation=1)


def signTops():
    height = 20.0
    width = height/2.0
    depth = 3

    signTop = cmds.polyCube(w=width, d=depth, h=height, ch=False)
    cmds.move(height/2.0, signTop, moveY=True)
    boolObj = cmds.duplicate(signTop)
    cmds.polyExtrudeFacet('.f[0]', lsy=0.85, lsx=0.75)
    cmds.polyExtrudeFacet('.f[0]', ltz=-depth/4.0)
    cmds.polyBevel(signTop)
    return signTop, boolObj


def makeTrash():
    tMin = cmds.intField('tMin', query=True, value=True)
    tMax = cmds.intField('tMax', query=True, value=True)
    faceList = list(range(0, 575))
    edgesList = list(range(0, 1199))
    vtxList = list(range(0, 624))
    tNum = cmds.intSlider('tNum', query=True, value=True)
    moveList = list(range(tMin, tMax))
    seed = random.randrange(sys.maxsize)
    random.Random(seed)

    planeList = []
    cmds.softSelect(sse=1)
    cmds.softSelect(sse=1, ssd=50.0, ssc='0,1,2,1,0,2', ssf=2)

    for x in range(2):
        plane = cmds.polyPlane(w=400, h=400, sx=24, sy=24, ch=False, name="trash_#")
        plane = plane[0]
        for x in range(tNum):
            itemSel = random.choice(faceList)
            faceList.remove(itemSel)
            itemSel = str(itemSel)
            itemSel = '.f[' + itemSel + ']'
            moveSel = random.choice(moveList)
            cmds.move(moveSel, itemSel, moveY=True, relative=True)
            cmds.polySoftEdge(itemSel, ws=1)
        """
        for x in range(tNum):
            itemSel = random.choice(vtxList)
            vtxList.remove(itemSel)
            itemSel = str(itemSel)
            itemSel = '.f[' + itemSel + ']'
            moveSel = random.choice(moveList)
            cmds.move(moveSel, itemSel, moveY=True, relative=True)
        """
        for x in range(tNum):
            itemSel = random.choice(edgesList)
            edgesList.remove(itemSel)
            itemSel = str(itemSel)
            itemSel = '.f[' + itemSel + ']'
            itemSel = cmds.select(plane + itemSel)
            moveSel = random.choice(moveList)
            cmds.move(moveSel, itemSel, moveY=True, relative=True)
        cmds.softSelect(sse=0)
        planeList.append(plane)
    cmds.polyCBoolOp(planeList[0], planeList[1], operation=1, classification=1, ch=False)
    cmds.select(clear=True)