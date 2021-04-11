import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import TimelineView from '../../assets/timeline-view.png';

const styles = {
    content: {
        height: '100%',
        width: '100%',
        display: 'flex',
        justifyContent: 'flex-start',
        flexDirection: 'column',
        alignItems: 'center',
        overflow: 'auto',
        borderRadius: '0',
    },
    title: {
        fontSize: '50px',
        fontWeight: 'bold',
        marginTop: '1%',
        alignSelf: 'center',
        color: '#33A1FD',
        textAlign: 'center',
    },
    text: {
        textAlign: 'center',
        padding: '1%',
    },
}

class EventTimeline extends React.Component {
    
render() {
        return(
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Timeline View</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The Timeline view allows users to view ShellBag Events in chronological order.
                </Typography>
                <img src={TimelineView} alt="timeline-view" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The Timeline view starts at the lowest zoom level, providing users with a chronologically 
                    sorted activity histogram that allows the user to quickly identify time periods of high 
                    and low levels of activity on the machine.
                </Typography>
                <img src={TimelineView} alt="timeline-view" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    The user can narrow the timespan being viewed by using the mouse scroll wheel:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. While hovering the cursor over the area they want to expand, the user can move the 
                    mouse scroll wheel up to zoom in and shorten the viewable timespan and enlarge the cards.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. As the user zooms in, the Shellbag event cards will sort themselves so that the 
                    viewable cards fall within the timespan area focused on by the user.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. By zooming in further, the user can further narrow the timespan and Shellbag 
                    Event data will begin to populate the cards.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    4. At this zoom level, the user can either further zoom in or navigate the timeline using the listed controls:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    a. To navigate left and right on the Timeline, the user can click and drag the Timeline left or right to navigate in either direction
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    i. Alternatively, holding the ALT key will allow the user to scroll left and right on the Timeline using the mouse scroll wheel
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    b. To navigate up and down on the Timeline, the user can click and drag the Timeline up or down to navigate in either direction
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    i. Alternatively, holding the CTRL key will allow the user to scroll up and down on the Timeline using the mouse scroll wheel
                </Typography>
                <img src={TimelineView} alt="timeline-view" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    5. To zoom out, the user can move the mouse wheel down and this will lengthen the viewable timespan 
                    and shrink the cards, allowing the user to view the activity histogram.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    6. Alternatively, users can filter the Shellbag Events on the Timeline using the Begin Date and End 
                    Date filters on the Filter Controls panel:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    a. Setting a Begin Date will filter the list for all Shellbag Events occurring after that date
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    b. Setting an End Date will filter the list for all Shellbag Events occurring before that date
                </Typography>
                <img src={TimelineView} alt="timeline-view" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    7. Using the Filter Controls, the user will be able to shorten the displayed timespan without needing to use the mouse scroll wheel controls.
                </Typography>
            </div>
            
        )
    }
}

export default withStyles(styles)(EventTimeline);