document.addEventListener("DOMContentLoaded", onLoaded);
document.addEventListener("keydown", onKeyDown);
document.addEventListener("mousedown", onDocumentMouseDown);

let currentDraggingElement;
let currentScalingElement;
let canDragInConstructorArea;
let constructorArea;

const draggingElementClassName = ".dragging";
const constructorElementClassName = ".drop-area";
const draggingElementInConstructorClassName = ".ctor-dragging";
const componentElementName = "image";


const dragStartEventName = "dragstart";
const dragEnterEventName = "dragenter";
const mouseDownEventName = "mousedown";
const mouseUpEventName = "mouseup";
const mouseMoveEventName = "mousemove";


function onLoaded() {
    setup_constructor_area();
    document.getElementById("createNew").addEventListener("click", createNew);
    document.getElementById("clear").addEventListener("click", clear);
}

function clear() {
    document.querySelector(constructorElementClassName).innerHTML = "";
}

function onDocumentMouseDown(e) {
    const elementUnderMouse = get_element_under_mouse(e);

    if (elementUnderMouse !== currentScalingElement) {
        currentScalingElement = null;
    }
}

async function createNew() {

    const components = document.querySelectorAll(".in-constructor");
    const name = document.getElementById("innogotchi_name").value;
    const result = [];

    components.forEach(c => {
        const translate = get_element_translate(c);
        const scale = get_element_scale(c);
        const imageUrl = c.getAttribute("href");
        const name = c.classList[0];
        result.push({
            translationX: translate.translateX,
            translationY: translate.translateY,
            scaleX: scale.scaleX,
            scaleY: scale.scaleY,
            imageUrl: imageUrl,
            name: name
        });
    });

    const viewModel = {
        name: name,
        components: result
    };

    const response = await window.fetch("/Constructor/Create",
        {
            method: "POST",
            headers: {
                'Content-type': "application/json"
            },
            body: JSON.stringify(viewModel)
        });

    if (response.ok) {
        const partial = document.getElementById("create_form");
        partial.innerHTML = await response.text();

        if (document.getElementById('pet_creation_error') === null) {
            clear();
        }
    }
}


function setup_constructor_area() {
    constructorArea = document.querySelector(constructorElementClassName);

    constructorArea.addEventListener(dragEnterEventName, onDragEnter);
    constructorArea.addEventListener(mouseDownEventName, onMouseDown);
    constructorArea.addEventListener(mouseUpEventName, onMouseUp);
    constructorArea.addEventListener(mouseMoveEventName, onMouseMove);

    constructorArea.removeChild(constructorArea.childNodes[0]);
}

function dragStart(e) {
    set_dragging_element(e);
}


function onDragEnter(e) {
    if (currentDraggingElement === null) return;

    place_component_in_constructor();
}

function onMouseDown(e) {
    set_can_drag_in_constructor_area(true);
    const elementUnderMouse = get_element_under_mouse(e);
    if (elementUnderMouse === null) {
        return;
    }

    currentDraggingElement = elementUnderMouse;
    currentScalingElement = elementUnderMouse;
}

function set_element_scale(element, newScaleX, newScaleY) {
    const curTrans = element.style.transform;
    const newScaleString = `scale(${newScaleX}, ${newScaleY})`;
    const regex = /scale\([0-9|\.]*\, [0-9|\.]*\)/;
    const newTrans = curTrans.replace(regex, newScaleString);
    element.style.transform = newTrans;
}

function onMouseUp(e) {
    set_can_drag_in_constructor_area(false);

    currentDraggingElement = null;
}

function onMouseMove(e) {
    if (!canDragInConstructorArea) return;

    const elementUnderMouse = currentDraggingElement;
    if (elementUnderMouse.tagName !== componentElementName) return;

    const currentTranslate = get_element_translate(elementUnderMouse);

    const newX = currentTranslate.translateX + e.movementX;
    const newY = currentTranslate.translateY + e.movementY;

    set_element_translate(elementUnderMouse, newX, newY);
}

function set_element_translate(element, translateX, translateY) {
    const curTrans = element.style.transform;
    const newTransformString = `translate(${translateX}px, ${translateY}px)`;
    const regex = /translate\(\-?[0-9|\.]*px\, \-?[0-9|\.]*px\)/;
    const newTrans = curTrans.replace(regex, newTransformString);
    element.style.transform = newTrans;
}

function set_can_drag_in_constructor_area(canDrag) {
    canDragInConstructorArea = canDrag;
}


function place_component_in_constructor() {
    const img = currentDraggingElement.querySelector("image");

    const className = img.className.baseVal;

    const existingChild = constructorArea.querySelector(`.${className}`);

    const copy = img.cloneNode(true);
    copy.classList.add("in-constructor");

    if (existingChild !== null) {
        constructorArea.insertBefore(copy, existingChild);
        constructorArea.removeChild(existingChild);
    } else {

        if (constructorArea.hasChildNodes() === false) {
            constructorArea.appendChild(copy);
            return;
        }

        const lastChild = constructorArea.lastChild;
        const previousChild = constructorArea.lastChild.previousSibling;
        const lastOrderInLayer = get_order_in_layer(lastChild);
        const insertingOrderInLayer = get_order_in_layer(copy);

        if (previousChild === null) {
            if (insertingOrderInLayer > lastOrderInLayer) {
                constructorArea.appendChild(copy);
                return;
            } else {
                constructorArea.insertBefore(copy, lastChild);
            }
        }

        const previousOrderInLayer = get_order_in_layer(previousChild);

        if (insertingOrderInLayer > lastOrderInLayer) {
            constructorArea.appendChild(copy);
            return;
        }

        if (insertingOrderInLayer < lastOrderInLayer && insertingOrderInLayer > previousOrderInLayer) {
            constructorArea.insertBefore(copy, lastChild);
            return;
        }

        constructorArea.insertBefore(copy, previousChild);
    }

    currentDraggingElement = null;
}

function get_order_in_layer(element) {
    return +element.getAttribute("orderinlayer");
}


function get_element_scale(element) {
    const style = window.getComputedStyle(element);
    const matrix = new DOMMatrixReadOnly(style.transform);
    return {
        scaleX: matrix.m11,
        scaleY: matrix.m22
    };
}


function get_element_translate(element) {
    const style = window.getComputedStyle(element);
    const matrix = new DOMMatrixReadOnly(style.transform);
    return {
        translateX: matrix.m41,
        translateY: matrix.m42
    };
}


function get_element_under_mouse(e) {
    return document.elementFromPoint(e.clientX, e.clientY);
}


function set_dragging_element(e) {
    currentDraggingElement = e.target;
}


function onKeyDown(e) {
    if (currentScalingElement === null || currentScalingElement === undefined) return;

    const scale = get_element_scale(currentScalingElement);
    const dScale = 0.1;

    if (e.key === "w") {
        set_element_scale(currentScalingElement, scale.scaleX, scale.scaleY + dScale);
    }
    if (e.key === "s") {
        set_element_scale(currentScalingElement, scale.scaleX, scale.scaleY - dScale);
    }
    if (e.key === "a") {
        set_element_scale(currentScalingElement, scale.scaleX - dScale, scale.scaleY);
    }
    if (e.key === "d") {
        set_element_scale(currentScalingElement, scale.scaleX + dScale, scale.scaleY);
    }
}