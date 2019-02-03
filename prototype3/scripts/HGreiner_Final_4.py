import maya.cmds as cmds
import random

"""
Notes:
y-direction is upwards
x is width 
z is depth
sx and sy flags are for subdivisions

"""


planeDict = {1: (list(range(15, 34)), list(range(5, 50))),
             2: (list(range(25, 174)), list(range(5, 205))),
             3: (list(range(35, 414)), list(range(10, 460))),
             4: (list(range(45, 754)), list(range(10, 815)))
             }


def terrainUI():
    """
    :argument:
    1. creates a window that is 125 units wide in a row/column layout style
    2. creates two integer sliders for the size of the plane and the amount of 'mountains' or 'dips' on the plane
    3. creates two integer fields for the min and max values for vertex movements in the positive y-direction
    4. creates a button that makes the plane through the command makePlane

    :return: nothing, the function does have a stores function in the 'Make' button that will create the planes when
        pressed

    :assumption: the user needs to press the created button for the UI to produce anything, the user will try any
        combination of settings
    """
    if cmds.window("terrainUI", exists=True):
        cmds.deleteUI("terrainUI", window=True)

    if cmds.windowPref("terrainUI", exists=True):
        cmds.windowPref('terrainUI', remove=True)

    cmds.window("terrainUI", title="Create Plane", minimizeButton=False, maximizeButton=False, sizeable=False)

    cmds.columnLayout("columnMain_C", parent="terrainUI")

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.text(label='')
    cmds.text(label='   Size    ', font="boldLabelFont", align='center')
    cmds.text(label='')
    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.intSlider('planeSize', minValue=1, maxValue=4, step=1, value=1)


    cmds.text("")
    cmds.rowColumnLayout(numberOfColumns=3, columnWidth=[(1, 90), (2, 25), (3, 10)], parent="columnMain_C")
    cmds.text(label='Max Height')
    cmds.intField('maxHeight', minValue=2, maxValue=5)
    cmds.text(label='')
    cmds.text(label='Min Height')
    cmds.intField('minHeight', minValue=1, maxValue=3)
    cmds.text(label='')


    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.separator(height=10, style="out")

    cmds.text(label='')
    cmds.text(label="Terrain Appearance")
    cmds.text(label='')
    cmds.rowColumnLayout(numberOfColumns=3, columnWidth=[(1, 50), (2, 25), (3, 50)], parent="columnMain_C")
    cmds.text(label='Less')
    cmds.text(label='')
    cmds.text(label='More')

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.intSlider('planePoint', minValue=1, maxValue=5, step=1, value=1)

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.separator(height=10, style="out")
    cmds.text(label='')
    cmds.text(label="Colors")
    cmds.text(label='')

    cmds.rowColumnLayout(numberOfColumns=4, columnWidth=[(1, 2.5), (2, 60), (3, 60), (4, 2.5)], parent="columnMain_C")
    cmds.radioCollection('Buttons')
    cmds.text(label='')
    cmds.radioButton('Purple', changeCommand=lambda args: shadingColor('Purple'))
    cmds.radioButton(label='Blue', changeCommand=lambda args: shadingColor('Blue'))
    cmds.text(label='')
    cmds.text(label='')
    cmds.radioButton(label='Green', changeCommand=lambda args: shadingColor('Green'))
    cmds.radioButton(label='Pink', changeCommand=lambda args: shadingColor('Pink'))
    cmds.text(label='')

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.text(label='')
    cmds.separator(height=10, style="out")
    cmds.text(label='')

    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], height=40, parent="columnMain_C")
    cmds.button(label='Make', height=40, command=lambda args: makePlane())


    cmds.rowColumnLayout(numberOfColumns=1, columnWidth=[(1, 125)], parent="columnMain_C")
    cmds.separator(height=10, style="out")

    cmds.showWindow("terrainUI")
  # dont forget this!!!! and put it at end


def planeSize():
    """
    :argument: checks to see the queried value of the intSlider 'planeSize' and stores a plane of that size, makes 1/2
        of a square sized plane

    :return: stored plane of a certain size depending on user input

    :assumption: the intSlider has produced an integer that is 1, 2, 3, or 4
    """
    plSize = cmds.intSlider('planeSize', query=True, value=True)
    name = "plane_#"
    divisions = 10 * plSize


    plSizeAlt = plSize * 4
    plane = cmds.polyPlane(w=plSizeAlt, h=plSizeAlt/2, sy=divisions/2, sx=divisions, ch=False, name=name)

    return plane


def lowpointRange():
    """
    :argument: checks to see the queried value of the intSlider 'planeSize' and identifies a list of numbers that
        represent face numbers for a certain plane from a dictionary

    :return: a list of a range of face number

    :assumption: the intSlider has produced an integer that is 1, 2, 3, or 4
    """
    plSize = cmds.intSlider('planeSize', query=True, value=True)
    listNumber = 0

    for listNumber in planeDict[plSize]:
        return listNumber


def pointRange():
    """
    :argument: checks to see the queried value of the intSlider 'planeSize' and identifies a list of numbers that
        represent vertex numbers for a certain plane from a dictionary

    :return: stored plane of a certain size depending on user input

    :assumption: the intSlider has produced an integer that is 1, 2, 3, or 4
     """
    plSize = cmds.intSlider('planeSize', query=True, value=True)
    listNumber = 1

    for listNumber in planeDict[plSize]:
        return listNumber


def addHighPoints(plSize, planePoint, minHeight, maxHeight):
    """
    :argument:
    1. chooses a certain number of vertexes depending on the 'planePoint' slider
    2. chooses an amount of points, the depends on the user's terrain appearance input, chosen from the dictionary list
        pulled in the pointRange() function
    3. the created list from different heights by using the user's input of the 'minHeight' and 'maxHeight'

    :return: a plane with vertexes moved in positive y-direction

    :assumption: all user selected options are integers
    """
    plSize = plSize * 4
    planePoint = planePoint * 5
    hRange = list(range(minHeight, maxHeight))
    highHeight = random.choice(hRange)
    lowHeight = random.choice(hRange)
    highList = []
    lowList = []

    plane = planeSize()
    plane = plane[0]

    for x in range(planePoint/2):
        choice = random.choice(pointRange())
        choice = str(choice)
        choice_face = plane + ".vtx[" + choice + "]"
        highList.append(choice_face)

    for x in range(planePoint/2):
        choice = random.choice(pointRange())
        choice = str(choice)
        choice_face = plane + ".vtx[" + choice + "]"
        lowList.append(choice_face)

    cmds.move(0, highHeight, 0, lowList, relative=True)
    cmds.move(0, lowHeight, 0, highList, relative=True)
    cmds.polySoftEdge(plane, a=0, ch=False)
    #cmds.displaySmoothness(plane, du=3, dv=3, pw=16, ps=4)
    cmds.move(0, 0, plSize / 4.0, plane, relative=True)
    return plane


def addLowPoints(plSize, planePoint):
    """
    :argument:
    1. chooses a certain number of faces depending on the 'planePoint' slider
    2. chooses an amount of points, the depends on the user's terrain appearance input, chosen from the dictionary list
        pulled in the lowpointRange() function

    :return: a plane with faces moved in negative y-direction

    :assumption: all user selected options are integers
    """
    plSize = plSize * 4
    planePoint = planePoint * 4
    pointList = []

    plane = planeSize()
    plane = plane[0]
    cmds.move(0, 0, -plSize / 4.0, plane, relative=True)

    for x in range(planePoint):
        choice = random.choice(lowpointRange())
        choice = str(choice)
        choice_face = plane + ".f[" + choice + "]"
        pointList.append(choice_face)

    cmds.move(0, -1, 0, pointList, relative=True)
    cmds.polySoftEdge(plane, a=0, ch=False)

    return plane


def shadingColor(color):
    """
    :argument: using selected radio buttons to change the default shading color

    :return: nothing, it just changes the color, is seen when there are new objects created in the scene

    :assumption: the user has shaders turned on
    """
    if color == 'Purple':
        blinn = cmds.shadingNode('blinn', asShader=True)
        cmds.setAttr(blinn + ".color", 0.14, 0.02, 0.22, type='double3')
        blinnSG = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name='blinnSG')
        cmds.connectAttr(blinn + ".outColor", blinnSG + ".surfaceShader", force=True)
        colorChosen = blinnSG
    if color == 'Green':
        blinn = cmds.shadingNode('blinn', asShader=True)
        cmds.setAttr(blinn + ".color", 0.009, 0.6, 0.03, type='double3')
        blinnSG = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name='blinnSG')
        cmds.connectAttr(blinn + ".outColor", blinnSG + ".surfaceShader", force=True)
        colorChosen = blinnSG
    if color == 'Blue':
        blinn = cmds.shadingNode('blinn', asShader=True)
        cmds.setAttr(blinn + ".color", 0.09, 0.5, 1.0, type='double3')
        blinnSG = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name='blinnSG')
        cmds.connectAttr(blinn + ".outColor", blinnSG + ".surfaceShader", force=True)
        colorChosen = blinnSG
    if color == 'Pink':
        blinn = cmds.shadingNode('blinn', asShader=True)
        cmds.setAttr(blinn + ".color", 1.0, 0.242, 1.242, type='double3')
        blinnSG = cmds.sets(renderable=True, noSurfaceShader=True, empty=True, name='blinnSG')
        cmds.connectAttr(blinn + ".outColor", blinnSG + ".surfaceShader", force=True)
        colorChosen = blinnSG

    cmds.setDefaultShadingGroup(colorChosen)
    # code referenced from Autodesk Maya's Command's page


def makePlane():
    """
    :argument: when the button is pressed it creates the two planes edited in previous functions

    :return: nothing, executes the code

    :assumption: all user input are between min and max values and are valid
    """
    plSize = cmds.intSlider('planeSize', query=True, value=True)
    planePoint = cmds.intSlider('planePoint', query=True, value=True)
    minHeight = cmds.intField('minHeight', query=True, value=True)
    maxHeight = cmds.intField('maxHeight', query=True, value=True)

    planeGroup = cmds.group(empty=True, world=True, name="terrain_#")
    plainGroup = []

    for x in range(1):
        plane = addHighPoints(plSize, planePoint, minHeight, maxHeight)
        cmds.parent(plane, planeGroup)
        plane = plane[0]
        plainGroup.append(plane)
    plainGroup.append(plane)

    for x in range(1):
        plane_2 = addLowPoints(plSize, planePoint)
        #cmds.color(plane_2, ud=2)
        cmds.parent(plane_2, planeGroup)
        plane_2 = plane_2[0]
        plainGroup.append(plane_2)
    plainGroup.append(plane_2)

    cmds.select(clear=True)