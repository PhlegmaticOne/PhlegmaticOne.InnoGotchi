document.addEventListener("DOMContentLoaded", onLoaded);

document.addEventListener('keypress', onKeyPress);

function onKeyPress(e) {
    if (e.key === 'k') {
        var svg = document.querySelector('.drop-area');
        svg.style.transform = 'scale(0.5)';
    }
}

let currentDraggingElement;
let canDragInConstructorArea;
let constructorArea;

const draggingElementClassName = '.dragging';
const constructorElementClassName = '.drop-area';
const draggingElementInConstructorClassName = '.ctor-dragging';
const componentElementName = 'image';


const dragStartEventName = 'dragstart';
const dragEnterEventName = 'dragenter';
const mouseDownEventName = 'mousedown';
const mouseUpEventName   = 'mouseup';
const mouseMoveEventName = 'mousemove';

function onLoaded() {
    setup_dragging_elements();
    setup_constructor_area();
}

function setup_dragging_elements() {
    const draggingItems = document.querySelectorAll(draggingElementClassName);

    draggingItems.forEach(draggingItem => {
        draggingItem.addEventListener(dragStartEventName, dragStart);
    });
}

function setup_constructor_area() {
    constructorArea = document.querySelector(constructorElementClassName);

    constructorArea.addEventListener(dragEnterEventName, onDragEnter);
    constructorArea.addEventListener(mouseDownEventName, onMouseDown);
    constructorArea.addEventListener(mouseUpEventName, onMouseUp);
    constructorArea.addEventListener(mouseMoveEventName, onMouseMove);
}

function dragStart(e) {
    set_dragging_element(e);
}

function onDragEnter(e) {
    place_component_in_constructor();
}

function onMouseDown(e) {
    set_can_drag_in_constructor_area(true);
    const elementUnderMouse = get_element_under_mouse(e);
    if (elementUnderMouse === null) return;

    currentDraggingElement = elementUnderMouse;

    if (e.button !== 2) return;

    var scale = get_element_scale(currentDraggingElement);
    changeScale(currentDraggingElement, scale.scaleX + 0.8, scale.scaleY + 0.8);
}

function changeScale(element, newScaleX, newScaleY) {
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

function get_element_under_mouse(e) {
    return document.elementFromPoint(e.clientX, e.clientY);
}

function set_element_translate(element, translateX, translateY) {
    const curTrans = element.style.transform;
    const newTransformString = `translate(${translateX}px, ${translateY}px)`;
    const regex = /translate\(\-?[0-9|\.]*px\, \-?[0-9|\.]*px\)/;
    const newTrans = curTrans.replace(regex, newTransformString);
    element.style.transform = newTrans;
}

function get_element_translate(element) {
    const style = window.getComputedStyle(element);
    const matrix = new DOMMatrixReadOnly(style.transform);
    return {
        translateX: matrix.m41,
        translateY: matrix.m42
    }
}

function get_element_scale(element) {
    const style = window.getComputedStyle(element);
    const matrix = new DOMMatrixReadOnly(style.transform);
    return {
        scaleX: matrix.m11,
        scaleY: matrix.m22
    }
}

function set_dragging_element(e) {
    currentDraggingElement = e.target;
}

function set_can_drag_in_constructor_area(canDrag) {
    canDragInConstructorArea = canDrag;
}

function place_component_in_constructor() {
    const img = currentDraggingElement.querySelector('image');
    const copy = img.cloneNode(true);
    constructorArea.appendChild(copy);
}