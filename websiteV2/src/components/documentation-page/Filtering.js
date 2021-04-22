import React from 'react';
import { withStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import EmptyFilter from '../../assets/empty-filter-controls.png';
import FilterTime from '../../assets/filter-time-selection.png';

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

class Filtering extends React.Component {

    render() {
        return (
            <div className={this.props.classes.content}>
                <Typography variant="title" className={this.props.classes.title}>Shellbag Filtering</Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Shellbag filter controls allow the user to pick a small range of shellbags to view at a time.
                </Typography>
                <img src={EmptyFilter} alt="shellbag-filtering" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    To use the filter controls:
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    1. Select a user of interest.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    2. Select a hive to filter from.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    3. Select a beginning and end date and time.
                </Typography>
                <img src={FilterTime} alt="shellbag-filtering-time-selector" />
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    4. All views should update to only display shellbags within the selected criteria.
                </Typography>
                <Typography variant="subtitle1" className={this.props.classes.text}>
                    Users can pick specific event types to look for in their filter controls. 
                    The user can filter by any of these fields independently, or narrow filters as much as they would like.
                </Typography>
            </div>
        )
    }
}

export default withStyles(styles)(Filtering);