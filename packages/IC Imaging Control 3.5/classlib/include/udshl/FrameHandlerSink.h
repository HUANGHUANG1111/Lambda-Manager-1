
#ifndef FRAMEHANDLERSINK_H_INC_
#define FRAMEHANDLERSINK_H_INC_

#pragma once

#include "GrabberSinkType.h"
#include "FrameFilter.h"
#include "MemBufferCollection.h"

namespace udshl
{
    class CFrameHandlerSinkCB;
}

namespace _DSHOWLIB_NAMESPACE
{
	class FrameHandlerSink;

	typedef smart_ptr<FrameHandlerSink> tFrameHandlerSinkPtr;

	/** This class represents a frame sink, which can deliver frames like the old FrameGrabberSink
	 * but also allows usage of FrameFilters for pre-filtering or transforming frames.
	 *
	 * @Concepts MemBufferCollection :
	 *	The MemBufferCollection is used to present the user asynchronously the frame
	 *	data in frameReady. The data is saved in the MemBufferCollection, until all other buffers are either
	 *	overwritten or locked. So each n = pCol->getBufferCount() times the buffer is overwritten, when it is not locked.
	 *
	 * @Concepts IFrameFilter
	 *	The frame filter must copy the data to the destination MemBuffer as presented in the dest frame in
	 *	its transform method. The src frame contains the stream data. When a FrameUpdateFilter is set, then 
	 *	it copies the frames as needed.
	 *
	 *	When no frame filter is registered then the data is copied by the standard handler, which does
	 *	copy the data without changing it, and so does not allow color space transforms.
	 *
	 *
	 * You can set 2 different frame types in the sink.
	 *	1. The accepted input type, is the frame type which is streamed into the sink.
	 *	2. The MemBufferCollection (if set) frame type, which defines the output type of the sink and so the
	 *			input type of frameReady.
	 *
	 *		When no input type is specified, the sink takes the MemBufferCollection frame type. If this
	 *			is not available, then the stream input type is taken (the type that is passed directly to the sink).
	 *		When no MemBufferCollection type is set (but a collection was requested) then it takes the input frame type
	 *			(either the enforced or the one implicitly set by the connection).
	 */
    UDSHL_DEPRECATE_FUNCTION_T_( "use FrameQueueSink, FrameSnapSink or FrameNotificationSink" )
	class FrameHandlerSink : public GrabberSinkType
	{
	public:
		typedef tFrameHandlerSinkPtr tFHSPtr;

		struct tCreateData {
			explicit tCreateData( unsigned int countBuffers );
			tCreateData( IFrameFilter* pFilter, unsigned int countBuffers = 0 );
			tCreateData( const tFrameFilterList& lst, unsigned int countBuffers = 0 );

			tCreateData( IFrameFilter* pFilter, const FrameTypeInfo& type, unsigned int countBuffers );
			tCreateData( const tFrameFilterList& lst, const FrameTypeInfo& type, unsigned int countBuffers );
			tCreateData( IFrameFilter* pFilter, const smart_ptr<MemBufferCollection>& pCol );
			tCreateData( const tFrameFilterList& lst, const smart_ptr<MemBufferCollection>& pCol );

			tCreateData( const smart_ptr<MemBufferCollection>& pCol );
			tCreateData( const FrameTypeInfo& type, unsigned int countBuffers );
			tCreateData( const FrameTypeInfoArray& acceptedInputTypes, unsigned int countBuffers );

			unsigned int					m_BufferCount;

			dvector<FrameTypeInfo>			m_FrameTypes;

			smart_ptr<MemBufferCollection>	m_pCollection;

			dvector<IFrameFilter*>			m_filterChain;

            FrameTypeInfoArray				getFrameTypeInfoArray() const { return FrameTypeInfoArray( m_FrameTypes ); }
            tFrameFilterList				getFilterChain() const { return tFrameFilterList( m_filterChain.begin(), m_filterChain.end() ); }

			bool operator!=( const tCreateData& op ) const
			{
				return (m_BufferCount != op.m_BufferCount)
					|| (m_FrameTypes != op.m_FrameTypes )
					|| (m_pCollection != op.m_pCollection)
					|| (m_filterChain != op.m_filterChain);
			}
		};

		/** Creates a handler chain, while not specifying the type of the stream. This means that a handler must support the
		 * incoming type whatever it is, because it cannot reject it.
		 * @param pFilter				The handler for the transform/copy operation.
		 * @param countBuffers			The number of buffers to create.
		 * @return	!= 0 when this chain could be created. 
		 *			== 0 when the chain could not be setup because one input/output frame type combination was invalid.
		 */
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( unsigned int countBuffers );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( IFrameFilter* pFilter, unsigned int countBuffers = 0 );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const tFrameFilterList& lst, unsigned int countBuffers = 0 );

		/** Creates a handler chain by setting the type of the MemBufferCollection. This means that the input type 
		 * of the sink will be the one specified by the collection/collection type. So the handler must implement a copy
		 * operation from the collection type to the collection type.
		 * @param pFilter				The handler for the transform/copy operation.
		 * @param countBuffers			The number of buffers to create.
		 * @param type					The type of buffers to create. The transform handler must support this frame type
		 *									as the input and output type.
		 * @param pCol					The collection to set. The transform handler must support the frame type of the
		 *									collection as an input and output type.
		 * @return	!= 0 when this chain could be created. 
		 *			== 0 when the chain could not be setup because one input/output frame type combination was invalid.
		 */
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( IFrameFilter* pFilter, const FrameTypeInfo& type, unsigned int countBuffers );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const tFrameFilterList& lst, const FrameTypeInfo& type, unsigned int countBuffers );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( IFrameFilter* pFilter, const smart_ptr<MemBufferCollection>& pCol );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const tFrameFilterList& lst, const smart_ptr<MemBufferCollection>& pCol );

		/** Creates a FrameHandlerSink, by specifying the type of the MemBufferCollection. No Handler is used.
		 * @param pCol					The collection to set.
		 * @param acceptedInputTypes	The collection of input types to be accepted.
		 * @param type					The type of buffers to create.
		 * @param countBuffers			The number of buffers to create.
		 * @return	!= 0 when this chain could be created. 
		 *			== 0 when the chain could not be setup because one input/output frame type combination was invalid.
		 */
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const smart_ptr<MemBufferCollection>& pCol );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const FrameTypeInfo& type, unsigned int countBuffers );
        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		static tFHSPtr	create( const FrameTypeInfoArray& acceptedInputTypes, unsigned int countBuffers );

        UDSHL_DEPRECATE_FUNCTION_T_( "use FrameNotificationSink or FrameQueueSink" )
		UDSHL_EXP_API_ static tFHSPtr	create( const tCreateData& data );
	public:
		UDSHL_EXP_API_ ~FrameHandlerSink();

		/** Changes the internal mode between Grab and Snap mode.
		 * In grab mode all frames reaching the sink are presented to the IFrameFilter and then  copied into the
		 * MemBufferCollection. After that the frameReady event of the GrabberListener is called.
		 *
		 * In snap mode all frames are presented to the IFrameFilter.
		 *	When no snap job is pending, then the destination frame in IFrameFilter::transform is 0 
		 *		and the frameReady event is not called.
		 *	When a snap job is pending, then a buffer is passed in a MemBuffer which will be presented in a
		 *		frameReady event in the GrabberListener, when IFrameFilter::transform returns true.
		 */
		UDSHL_EXP_API_ void		setSnapMode( bool bSnapModeOn );
		UDSHL_EXP_API_ bool		getSnapMode() const;

		/** Enqueues a snap job, which lets <count> frames to be copied into the MemBufferCollection.
		 * The method returns when either <count> frames are snapped, or the timeout elapses.
		 *
		 * When the method runs into the timeout, it is not guaranteed, that <count> frames are snapped.
		 * In case of a timeout it may happen that after the method returned, GrabberListener::frameReady may be called.
		 * 
		 * @param count		Number of frames to copy into the MemBufferCollection.
		 * @param timeout	
		 * @return
		 */
		UDSHL_EXP_API_ Error		snapImages( unsigned int count, DWORD timeout = INFINITE );
		/** Enqueues a snap job, which lets <count> frames to be copied into the MemBufferCollection.
		 * The method returns at once after queuing the job.
		 *
		 * You cannot terminate this snap job!!
		 * @param count		Number of frames to copy into the MemBufferCollection.
		 * @return
		 */
		UDSHL_EXP_API_ Error		snapImagesAsync( unsigned int count );

		/** Returns the current MemBufferCollection.
		 * @return 0 when no collection is used or the collection was not yet setup, otherwise the collection.
		 */
		UDSHL_EXP_API_ smart_ptr<MemBufferCollection>	getMemBufferCollection() const;

		/** Returns the currently set Transform CB.
		 * @return 0 when no handler is set, otherwise the handler.
		 */
		const tFrameFilterList			getFrameFilters() const
		{
			return to_vector( getFrameFilters_() );
		}

		/** Returns the MemBuffer in which the last frame was fully copied. Each restart of the sink does
		 * set the start of the MemBufferCollection to the first buffer. After stopping the sink, this holds
		 * the last acquired frame until the sink is restarted. Restarting happens, when you call Grabber::startLive.
		 * So this returns 0 when no frame was acquired up to now.
		 * @return 0 when no MemBufferCollection is set or when no buffer was copied yet.
		 */
		UDSHL_EXP_API_ smart_ptr<MemBuffer>			getLastAcqMemBuffer() const;

		/** Returns the frame type of the sink (either the frame type of the MemBufferCollection or
		 * the output frame type of the handler, when no MemBufferCollection is set).
		 *
		 * @param info	When the information is available, then info is filled with the output frame type of the sink.
		 * @return true when the information is available and info was filled with the according data, otherwise false.
		 */
		UDSHL_EXP_API_ bool							getOutputFrameType( FrameTypeInfo& info ) const;

		/** Applies a new MemBufferCollection to the sink. The sink
		 * tests if the new collection does function with the handler and the specified frame types.
		 * 
		 * This functions when the system is already running.
		 * @param pCol	The new collection. May be 0.
		 * @return true on success, false when the collection could not be set (e.g. the collection has
		 *						a frame type which the handler does not understand).
		 */
        UDSHL_EXP_API_ bool							setMemBufferCollection( const smart_ptr<MemBufferCollection>& pCol );
		/** Returns eFrameHandlerSink. */
		UDSHL_EXP_API_ tSinkType					getSinkType() const;

		/** Returns the number of frames acquired in the MemBufferCollection up to now, or in the last run. */
		UDSHL_EXP_API_ unsigned int					getFrameCount() const;

		UDSHL_EXP_API_ const tCreateData&			getCreateData() const;
	protected:
		UDSHL_EXP_API_ dvector<IFrameFilter*>			getFrameFilters_() const;

		FrameHandlerSink( const tCreateData& data, udshl::CFrameHandlerSinkCB* pCB, smart_com<IFrameFilter> pFilterChain );

		FrameHandlerSink( const FrameHandlerSink& op2 );		// copying disallowed
		void	operator=( const FrameHandlerSink& op2 );

		virtual smart_com<icbase::IDShowFilter>		getBaseSinkFilter() const;

		smart_com<icbase::IDShowFilter>	m_pFrameHandlerSink;
		udshl::CFrameHandlerSinkCB*	    m_pFrameHandlerSinkCB;

		tCreateData					m_CreateData;

		smart_com<IFrameFilter>		m_pFilterChain;

		virtual bool	attach( GrabberPImpl* );
		virtual void	detach();
	};

#if defined UDSHL_DEPRECATE_MESSAGES_ENABLED_
#   pragma warning( push )
#   pragma warning( disable : 4996 ) // error C4996: 'DShowLib::FrameHandlerSink::create': was declared deprecated. Instead, use the use FrameNotificationSink or FrameQueueSink.
#endif

	inline FrameHandlerSink::tCreateData::tCreateData( unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 )
	{
	}

	inline FrameHandlerSink::tCreateData::tCreateData( IFrameFilter* pFilter, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 )
	{
		m_filterChain.push_back( pFilter );
	}
	inline FrameHandlerSink::tCreateData::tCreateData( const tFrameFilterList& lst, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 ), m_filterChain( lst.begin(), lst.end() )
	{
	}

	inline FrameHandlerSink::tCreateData::tCreateData( IFrameFilter* pFilter, const FrameTypeInfo& type, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 )
	{
		m_FrameTypes.push_back( type );
		m_filterChain.push_back( pFilter );
	}
	inline FrameHandlerSink::tCreateData::tCreateData( const tFrameFilterList& lst, const FrameTypeInfo& type, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 ), 
		m_filterChain( lst.begin(), lst.end() )
	{
		m_FrameTypes.push_back( type );
	}

	inline FrameHandlerSink::tCreateData::tCreateData( IFrameFilter* pFilter, const smart_ptr<MemBufferCollection>& pCol )
		: m_BufferCount( 0 ), m_FrameTypes(), m_pCollection( pCol )
	{
		m_filterChain.push_back( pFilter );
	}
	inline FrameHandlerSink::tCreateData::tCreateData( const tFrameFilterList& lst, const smart_ptr<MemBufferCollection>& pCol )
		: m_BufferCount( 0 ), m_FrameTypes(), m_pCollection( pCol ), m_filterChain( lst.begin(), lst.end() )
	{
	}


	inline FrameHandlerSink::tCreateData::tCreateData( const smart_ptr<MemBufferCollection>& pCol )
		: m_BufferCount( 0 ), m_FrameTypes(), m_pCollection( pCol )
	{
	}
	inline FrameHandlerSink::tCreateData::tCreateData( const FrameTypeInfo& type, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes(), m_pCollection( 0 )
	{
		m_FrameTypes.push_back( type );
	}
	inline FrameHandlerSink::tCreateData::tCreateData( const FrameTypeInfoArray& acceptedInputTypes, unsigned int countBuffers )
		: m_BufferCount( countBuffers ), m_FrameTypes( acceptedInputTypes.to_dvector() ), m_pCollection( 0 )
	{
	}

	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( unsigned int countBuffers )
	{
		return create( tCreateData( countBuffers ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( IFrameFilter* pHandler, unsigned int countBuffers )
	{
		return create( tCreateData( pHandler, countBuffers ) );
	}

	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const tFrameFilterList& lst, unsigned int countBuffers )
	{
		return create( tCreateData( lst, countBuffers ) );
	}

	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const FrameTypeInfo& type, unsigned int countBuffers )
	{
		if( type.subtype == GUID_NULL )			return create( countBuffers );

		return create( tCreateData( type, countBuffers ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const smart_ptr<MemBufferCollection>& pCol )
	{
		if( pCol == 0 )							return 0;
		return create( tCreateData( pCol ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( IFrameFilter* pHandler, const FrameTypeInfo& type, unsigned int countBuffers )
	{
		return create( tCreateData( pHandler, type, countBuffers ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const tFrameFilterList& lst, const FrameTypeInfo& type, unsigned int countBuffers )
	{
		return create( tCreateData( lst, type, countBuffers ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( IFrameFilter* pHandler, const smart_ptr<MemBufferCollection>& pCol )
	{
		return create( tCreateData( pHandler, pCol ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const tFrameFilterList& lst, const smart_ptr<MemBufferCollection>& pCol )
	{
		return create( tCreateData( lst, pCol ) );
	}
	inline tFrameHandlerSinkPtr	FrameHandlerSink::create( const FrameTypeInfoArray& acceptedInputTypes, unsigned int countBuffers )
	{
		return create( tCreateData( acceptedInputTypes, countBuffers ) );
	}

#if UDSHL_DEPRECATE_MESSAGES_ENABLED_
#   pragma warning( pop )
#endif
};

#endif // FRAMEHANDLERSINK_H_INC_
