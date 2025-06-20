import React from "react";
import { NavLink } from "react-router-dom";
import PropTypes from "prop-types";
import SimpleBar from "simplebar-react";
import "simplebar-react/dist/simplebar.min.css";
import { CBadge, CNavLink, CSidebarNav } from "@coreui/react";
import { useState } from "react";
import { AddOrUpdateGroups, AddOrUpdateTask, SidebarGroups } from "../index";

const navLink = (name, icon, badge, indent = false) => {
    return (
        <>
            {icon
                ? icon
                : indent && (
                    <span className="nav-icon">
                        <span className="nav-icon-bullet"></span>
                    </span>
                )}

            {name && name}

            {badge && (
                <CBadge color={badge.color} className="ms-auto" size="sm">
                    {badge.text}
                </CBadge>
            )}
        </>
    );
};

const navItem = (item, index, indent = false, getSetModelVisibilityState) => {
    const { component, modelName, name, badge, icon, ...rest } = item;
    const setModelVisibilityState = getSetModelVisibilityState(modelName);
    const Component = component;
    return (
        <Component as="div" key={index}>
            {rest.to || rest.href ? (
                <CNavLink
                    {...(rest.to && { as: NavLink })}
                    {...(rest.href && { target: "_blank", rel: "noopener noreferrer" })}
                    {...rest}
                >
                    {navLink(name, icon, badge, indent)}
                </CNavLink>
            ) : modelName ? (
                <CNavLink
                    style={{ cursor: "pointer" }}
                    {...(setModelVisibilityState && { onClick: () => setModelVisibilityState(true) })}
                >
                    {navLink(name, icon, badge, indent)}
                </CNavLink>
            ) : (
                navLink(name, icon, badge, indent)
            )}
        </Component>
    );
};

const navGroup = (item, index) => {
    const { component, name, icon, items, ...rest } = item;
    const Component = component;
    return (
        <Component
            compact
            as="div"
            key={index}
            toggler={navLink(name, icon)}
            {...rest}
        >
            {items?.map((item, index) =>
                item.items ? navGroup(item, index) : navItem(item, index, true)
            )}
        </Component>
    );
};

export const AppSidebarNav = ({ items }) => {
    const [visibleModelForGroupAdd, setVisibleModelForGroupAdd] = useState(false);
    const [visibleModelForTaskAdd, setVisibleModelForTaskAdd] = useState(false);

    const getSetModelVisibilityState = (modelName) => {
        switch (modelName) {
            case "AddOrUpdateGroups":
                return setVisibleModelForGroupAdd;

            case "AddOrUpdateTask":
                return setVisibleModelForTaskAdd;
            default:
                return "";
        }
    };
    const ReturnComponent = (component, index) => {
        switch (component) {
            case "SidebarGroups":
                return <SidebarGroups key={index} />;
            default:
                return null;
        }
    };

    return (
        <>
            <CSidebarNav as={SimpleBar}>
                {items &&
                    items.map((item, index) =>
                        item.name === "ComponentName" && !item.items
                            ? ReturnComponent(item.component, index)
                            : item.items
                                ? navGroup(item, index)
                                : navItem(item, index, false, getSetModelVisibilityState)
                    )}
            </CSidebarNav>

            {visibleModelForGroupAdd && (
                <AddOrUpdateGroups
                    visible={visibleModelForGroupAdd}
                    setVisibility={setVisibleModelForGroupAdd}
                />
            )}

            {visibleModelForTaskAdd && (
                <AddOrUpdateTask
                    visible={visibleModelForTaskAdd}
                    setVisibility={setVisibleModelForTaskAdd}
                    taskId={0}
                    groupId={0}
                    isStarredTask={false}
                />
            )}
        </>
    );
};

AppSidebarNav.propTypes = {
    items: PropTypes.arrayOf(PropTypes.any).isRequired,
};
